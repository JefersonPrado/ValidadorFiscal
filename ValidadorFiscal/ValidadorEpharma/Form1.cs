using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace ValidadorFiscal
{


    public partial class Form1 : Form
    {   //Variaveis globais 

        String CaminhoSalvartxtErros, NomedoArquivoReferencia, caminhoTexto;
        Boolean debuga = true;
        string spc = @"\\-----------------------//";
        static int a;
        static string pastaBancoPendente = @"C:\BANCOS\PENDENTES";
        static string pastaBancoRestaurado = @"C:\BANCOS\RESTAURADOS";

        private void Form1_Load(object sender, EventArgs e)

        { notifyIcon1.BalloonTipTitle = "FISCAL ECO";
            notifyIcon1.BalloonTipText = "FISCAL ECO";
            notifyIcon1.Text = "FISCAL ECO";
      
            var contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(new MenuItem("Sair", sair_click));
            contextMenu.MenuItems.Add(new MenuItem("Mover Backup", MoveDirectory_click));
            notifyIcon1.ContextMenu = contextMenu;


            TXTInstancia.Text = Properties.Settings.Default.instancia;
            TXTUsuario.Text = Properties.Settings.Default.usuario;
            TXTSenha.Text = Properties.Settings.Default.senha;
            TXTPastaData.Text = Properties.Settings.Default.pastaData;

            string caminhoData = Properties.Settings.Default.pastaData;

            DesativaCampos(false);
            EscutaPastaBanco(pastaBancoPendente);
            Console.WriteLine("Load: " + pastaBancoPendente);

          
        }

        private void sair_click(object sender, EventArgs e) {
            Application.Exit();
        }
        public Form1()
        {
            InitializeComponent();
           
        }
        private static Server Connecta()
        {
            ServerConnection conn = new ServerConnection(Properties.Settings.Default.instancia, Properties.Settings.Default.usuario, Properties.Settings.Default.senha);
            Server myServer = new Server(conn);
            return myServer;
        }
        private void RestoreDataBase(string CaminhoPastaBackup,
            string NovoNomeBanco,
            string PastaData,
            string DatabaseFileName,
            string DatabaseLogFileName)
        {
            Server myServer = Connecta();
            Restore myRestore = new Restore();
            myRestore.Database = NovoNomeBanco;
            Database currentDb = myServer.Databases[NovoNomeBanco];
            if (currentDb != null)
                myServer.KillAllProcesses(NovoNomeBanco);
            myRestore.Devices.AddDevice(CaminhoPastaBackup, DeviceType.File);
            string DataFileLocation = PastaData + "\\" + NovoNomeBanco + ".mdf";
            string LogFileLocation = PastaData + "\\" + NovoNomeBanco + "_log.ldf";
            myRestore.RelocateFiles.Add(new RelocateFile(DatabaseFileName, DataFileLocation));
            myRestore.RelocateFiles.Add(new RelocateFile(DatabaseLogFileName, LogFileLocation));
            myRestore.ReplaceDatabase = true;
            myRestore.PercentCompleteNotification = 10;
            myRestore.PercentComplete += new PercentCompleteEventHandler(myRestore_PercentComplete);
      
            myRestore.Complete += new ServerMessageEventHandler(myRestore_Complete);
            Console.WriteLine("Restoring:{0}", NovoNomeBanco);
            //myServer.BackupDirectory = "";
            myRestore.SqlRestore(myServer);
            currentDb = myServer.Databases[NovoNomeBanco];
            currentDb.SetOnline();

        }

        static void myRestore_Complete(object sender, Microsoft.SqlServer.Management.Common.ServerMessageEventArgs e)
        {
            MoveDirectory(pastaBancoPendente, pastaBancoRestaurado);

        }

        private void myRestore_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
           // Console.WriteLine(e.Percent.ToString() + "% Complete");
          
            //progressBar1.Value = Convert.ToInt32(e.Percent.ToString());
            progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = Convert.ToInt32(e.Percent.ToString())));
            LBStatus.Invoke((MethodInvoker)(() => LBStatus.Text = "Ultimo banco blalb alkblal"));
        }
        private void BTNConfig_Click(object sender, EventArgs e)
        {
          
        }
        private void OnChangedBanco2(object source, FileSystemEventArgs e)
        {
            Console.WriteLine(Spc("Entrou na função: OnChangedBanco2"));
           
            string[] diretorios = Directory.GetDirectories(pastaBancoPendente);

            // Esse comando retorna o nome do arquivo somente.            
            string novoNomeBanco = (Path.GetFileName(e.FullPath));

            var arq1 = Directory.EnumerateFiles(e.FullPath, "*.bak", SearchOption.AllDirectories);
            List<string> ar = new List<string>(arq1);

            ArrayList listArq = new ArrayList(arq1.ToArray());
            foreach (var item in listArq)
            {
                Console.WriteLine("Arquivo completo em: " + item);
            }


            Console.WriteLine(e.FullPath);
            string zx = listArq[0].ToString();

            Console.WriteLine("Somente a pasta: " + Path.GetFileName(e.FullPath));
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("Nome do banco de dados:" + novoNomeBanco);
            Console.WriteLine("Caminho completo do .bak: " + zx + "");
            //
            List<string> ts = MontarStringePasta(zx);

            Console.WriteLine(Spc("OnChangedBanco >" + ts[0].ToString()));
            Console.WriteLine(Spc("OnChangedBanco >" + ts[1].ToString()));

            //RestaurarBancoScript(zx,
            //    Properties.Settings.Default.pastaData,
            //    novoNomeBanco, ts[0].ToString(), ts[1].ToString());

            RestoreDataBase(zx,Path.GetFileName(e.FullPath), Properties.Settings.Default.pastaData, ts[0].ToString(), ts[1].ToString());


            Console.WriteLine(Spc("Saiu da função: OnChangedBanco"));
        }

        public void EscutaPastaBanco(string caminhoBancoPendente)
        {
            Console.WriteLine(Spc("Entrou na função: EscutaPastaBanco"));

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = caminhoBancoPendente;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
            | NotifyFilters.FileName | NotifyFilters.DirectoryName;     
            watcher.Created += new FileSystemEventHandler(OnChangedBanco2);

            if (Directory.Exists(caminhoBancoPendente))
            {
                watcher.EnableRaisingEvents = true;
                Console.WriteLine(Spc("Saiu da função: EscutaPastaBanco"));
            }
            else{
                Console.WriteLine("Caminho não foi definido");
            }
        }
        private void OnChangedBanco(object source, FileSystemEventArgs e)
        {
            Console.WriteLine(Spc("Entrou na função: OnChangedBanco"));
            string[] diretorios = Directory.GetDirectories(pastaBancoPendente);

            // Esse comando retorna o nome do arquivo somente.            
            string novoNomeBanco = (Path.GetFileName(e.FullPath));

            var arq1 = Directory.EnumerateFiles(e.FullPath, "*.bak", SearchOption.AllDirectories);
            List<string> ar = new List<string>(arq1);

            ArrayList listArq = new ArrayList(arq1.ToArray());
            foreach (var item in listArq)
            {
                Console.WriteLine("Arquivo completo em: " + item);
            }


            Console.WriteLine(e.FullPath);
            string zx = listArq[0].ToString();

            Console.WriteLine("Somente a pasta: " + Path.GetFileName(e.FullPath));
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("Nome do banco de dados:" + novoNomeBanco);
            Console.WriteLine("Caminho completo do .bak: " + zx + "");
            //
            List<string> ts = MontarStringePasta(zx);

            Console.WriteLine(Spc("OnChangedBanco >" + ts[0].ToString()));
            Console.WriteLine(Spc("OnChangedBanco >" + ts[1].ToString()));

            //RestaurarBancoScript(zx,
            //    Properties.Settings.Default.pastaData,
            //    novoNomeBanco, ts[0].ToString(), ts[1].ToString());
            //Contador();

            Console.WriteLine(Spc("Saiu da função: OnChangedBanco"));
        }

        //Função para encontrar as pastas e arquivos, retorna pasta e arquivos achados neles
        public (string[], string) EncontrarArquivosNaPastas(string pasta)
        {   // localizar arquivo com nome correcao_
            string textToRemove = "Correcao_";
            //Comando para combinar strings - pensei em combinar pasta + noome do arquivo.
            string caminhoTexto = System.IO.Path.Combine(pasta);
            //Comando para colocar no array as pastas encontradas no cominho.
            string[] diretorios = Directory.GetDirectories(caminhoTexto);
            var arquivos = Directory.EnumerateFiles(caminhoTexto, "*.txt", SearchOption.AllDirectories);        

            ArrayList listaDiretorios = new ArrayList(diretorios);
            ArrayList listaArquivosEncontrados = new ArrayList(arquivos.ToArray());

            Console.WriteLine("//"+listaArquivosEncontrados[1]);

            foreach (var item2 in listaDiretorios)
            {
                Console.WriteLine("Pastas encontradas " + item2);

            }

            //laço de repetição para  identificar apenas arquivos não corrigidos
           for(int icont =0; icont < listaDiretorios.Count; icont++)
            {
                Console.WriteLine("Valor de icont " + icont );

                for (int jcont = 0; jcont < listaArquivosEncontrados.Count; jcont++)
                {

                    Console.WriteLine("Valor de jcont "+ jcont);

                    int index = listaArquivosEncontrados.IndexOf(textToRemove.ToString());
                    if (listaArquivosEncontrados.Contains(textToRemove))
                    {                     
                
                    
                        Console.WriteLine("ACHEI O ARQUIVO");
                        listaDiretorios.RemoveAt(index);
                        listaArquivosEncontrados.RemoveAt(jcont);
                }              
            }
               

            }
            //listaDiretorios.Add("dog");
            //listaDiretorios.RemoveAt(1);

            for (int i = 0; i < listaArquivosEncontrados.Count; i++)
            {
                Console.WriteLine("Aquivo com arquivo pendente de correção " + listaArquivosEncontrados[i]);
            }

          

            foreach (var item2 in listaDiretorios)
            {
                Console.WriteLine("Pasta com arquivo pendente de correção "+item2);

            }


         
            return (diretorios, arquivos.ToString());

        }
    
        private void ExecutarBackground_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {       

            StreamWriter leitordeTexto;
            string[] readText = File.ReadAllLines(caminhoTexto);
            string[] cestarray = { "|0000000|", "|1111111|", "|2222222|","|3333333|", "|4444444|", "|5555555|"
                , "|6666666|", "|7777777|", "|8888888|", "|9999999|"};

            for(int l = 0; l < readText.Length; l++) {
                //SE CONTER UMA INFORMAÇÃO EM ESPECIFICO
                for (int i = 0; i < cestarray.Length; i++)
                { if (readText[l].Contains(cestarray[i]))
                    {
                        //Alterar a informação na linha;
                        readText[l] = readText[l].Replace(cestarray[i], "|1300200|");
                        //GRAVAR NO NOVO ARQUIVO DE TEXTO;                      
                    }              

                }
            }
            CriarDocumentoCorrigido(readText);
        }           
        public void CriarDocumentoCorrigido(String[] text)
        {
            String caminho = (CaminhoSalvartxtErros + "\\Correcao_" + NomedoArquivoReferencia);

            if (File.Exists(caminho))
            {File.Delete(caminho);
                //   MessageBox.Show("ARQUIVO ELIMINADO COM SUCESSO");
                //Criar um arquivo Novo com a correção.
                File.WriteAllLines(caminho, text, Encoding.UTF8);               
            }
            else
            { //Criar um arquivo Novo com a correção.
                File.WriteAllLines(caminho, text, Encoding.UTF8);
            }
          //  return "C:\\TEMP\\SPEDFiscal_01-01-2022_31-01-2022_CNPJ_00730668000194.txt";
        }

        private void MoveDirectory_click(object sender, EventArgs e)
        {
            MoveDirectory(pastaBancoPendente, pastaBancoRestaurado);
        }
        
        public static void MoveDirectory(string sourceFile, string destFile )
        {
            string nomearquivo;
            Console.WriteLine(("Entrou na função: MoveDirectory"));

            var diretorios =Directory.EnumerateDirectories(sourceFile);

            List<string> lt = diretorios.ToList();
            // Esse comando retorna o nome do arquivo somente.            
          

            foreach (string dir in lt)
            {
                Console.WriteLine("Mover de "+dir.ToString()+" Para "+destFile + dir);
              
                nomearquivo = (Path.GetFileName(dir));
                Console.WriteLine(("Arquivo Nome "+nomearquivo));

                Directory.Move(dir, Path.Combine(destFile, nomearquivo));
            }

           

            //string fileName = "test.txt";
            //string sourcePath = @"C:\BANCOS\PENDENTES";
            //string targetPath = @"C:\BANCOS\RESTAURADOS";

            ////string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            ////string destFile = System.IO.Path.Combine(targetPath, fileName);

            //Directory.Move(@"C:\BANCOS\PENDENTES\INOVA_EMITE_FISCAL_74874", @"C:\BANCOS\RESTAURADOS");

            //if (Directory.Exists(sourcePath))
            //{
            //    string[] files = System.IO.Directory.GetFiles(sourcePath);

            //    // Copy the files and overwrite destination files if they already exist.
            //    foreach (string s in files)
            //    {
            //        // Use static Path methods to extract only the file name from the path.
            //        fileName = System.IO.Path.GetFileName(s);
            //        destFile = System.IO.Path.Combine(targetPath, fileName);
            //        System.IO.File.Copy(s, destFile, true);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Source path does not exist!");
            //}
            Console.WriteLine(("Saiu da função: MoveDirectory"));
        }
        public string informarCaminhoSalvar()
        {
            if (debuga)
            {
                return  "C:\\TEMP\\SPEDFiscal_01-01-2022_31-01-2022_CNPJ_00730668000194.txt";          
            }
            //Abrir caixa de seleção
            OpenFileDialog saveCaminho = new OpenFileDialog();
            //informar o nome do filtro e a extenção que queremos salvar
            saveCaminho.Filter = "Arquivo Texto |*.txt";
            //mostrar a caixa de dialogo
            saveCaminho.ShowDialog();

            if (saveCaminho.FileName == null || saveCaminho.FileName == "")
            {
              
            }
            else
            {
                
                //Pegar somente o caminho do arquivo 
                FileInfo dadosArquivo = new FileInfo(saveCaminho.FileName);
                CaminhoSalvartxtErros = dadosArquivo.DirectoryName;
                NomedoArquivoReferencia = dadosArquivo.Name;

            }
            return saveCaminho.FileName;
        }
        public string Spc(string text)
        {

            string txtTratado;
            txtTratado = spc + text + spc;
            return txtTratado;
        }
        public List<string> MontarStringePasta(string caminhoDoBak)
        {
            
            string sql = "";
            sql = "RESTORE FILELISTONLY FROM DISK='" + caminhoDoBak + "' WITH FILE=1";
            Console.WriteLine(Spc("Busando pelo nome do arquivo logico e fisico de banco de dados"));
            Console.WriteLine(sql);

            List<string> lt = EncontrarNomeLogico(sql, ConectaScript());
            Console.WriteLine("Conferir se esta certo" + lt[0].ToString());
            Console.WriteLine(lt[1].ToString());

            return lt;
        }
        private static string ConectaScript()
        {
            string sql = @"Server="+Properties.Settings.Default.instancia+
                "; User Id="+Properties.Settings.Default.usuario+"; Password="+
                Properties.Settings.Default.senha+";";
            Console.WriteLine(sql);
                return sql;
           
        }

        private List<string> EncontrarNomeLogico(string queryString, string connectionString)
        {       using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();

                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = command.ExecuteReader();
                List<string> list = (from IDataRecord r in reader select (string)r["LogicalName"]).ToList();
                Console.WriteLine(Spc("Nomes Logicos Encontrados"));
                Console.WriteLine(list[0].ToString());
                Console.WriteLine(list[1].ToString());

                while (reader.Read())
                {
                    Console.WriteLine(String.Format("{0}{1}", reader[0], reader[1]));
                }

                return list;
            }
        }      
        private void notifyIcon1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {   this.Show();
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;


        }
        private void Form1_Resize_1(object sender, EventArgs e)
        {if (WindowState == FormWindowState.Minimized)
            {   this.Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000);
            }
            else if (FormWindowState.Normal == this.WindowState)
            {notifyIcon1.Visible = false;
            }
        }
        private void BTNSalvar2_Click(object sender, EventArgs e)
        {

            Properties.Settings.Default.instancia = TXTInstancia.Text;

            Properties.Settings.Default.usuario = TXTUsuario.Text;

            Properties.Settings.Default.senha = TXTSenha.Text;

            Properties.Settings.Default.pastaData = TXTPastaData.Text;

            Properties.Settings.Default.Save();

            LBStatus.Text = "Salvo com sucesso!";
            DesativaCampos(false);
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void BTNSQLBonito_Click(object sender, EventArgs e)
        {
            //text();
            var s = TXTSqlEntrada.Text;
            if (s == null)
            {

            }
            else
            {
                TXTSqlSaida.Text = SQLEnbelezado(s);


            }


        }
        public void text()
        {
            string str;
            string nl = Environment.NewLine;
            //
            Console.WriteLine();
            Console.WriteLine("-- Environment members --");

            //  Invoke this sample with an arbitrary set of command line arguments.
            Console.WriteLine("CommandLine: {0}", Environment.CommandLine);

            string[] arguments = Environment.GetCommandLineArgs();
            Console.WriteLine("GetCommandLineArgs: {0}", String.Join(", ", arguments));

            //  <-- Keep this information secure! -->
            Console.WriteLine("CurrentDirectory: {0}", Environment.CurrentDirectory);

            Console.WriteLine("ExitCode: {0}", Environment.ExitCode);

            Console.WriteLine("HasShutdownStarted: {0}", Environment.HasShutdownStarted);

            //  <-- Keep this information secure! -->
            Console.WriteLine("MachineName: {0}", Environment.MachineName);

            Console.WriteLine("NewLine: {0}  first line{0}  second line{0}  third line",
                                  Environment.NewLine);

            Console.WriteLine("OSVersion: {0}", Environment.OSVersion.ToString());

            Console.WriteLine("StackTrace: '{0}'", Environment.StackTrace);

            //  <-- Keep this information secure! -->
            Console.WriteLine("SystemDirectory: {0}", Environment.SystemDirectory);

            Console.WriteLine("TickCount: {0}", Environment.TickCount);

            //  <-- Keep this information secure! -->
            Console.WriteLine("UserDomainName: {0}", Environment.UserDomainName);

            Console.WriteLine("UserInteractive: {0}", Environment.UserInteractive);

            //  <-- Keep this information secure! -->
            Console.WriteLine("UserName: {0}", Environment.UserName);

            Console.WriteLine("Version: {0}", Environment.Version.ToString());

            Console.WriteLine("WorkingSet: {0}", Environment.WorkingSet);

            //  No example for Exit(exitCode) because doing so would terminate this example.

            //  <-- Keep this information secure! -->
            string query = "My system drive is %SystemDrive% and my system root is %SystemRoot%";
            str = Environment.ExpandEnvironmentVariables(query);
            Console.WriteLine("ExpandEnvironmentVariables: {0}  {1}", nl, str);

            Console.WriteLine("GetEnvironmentVariable: {0}  My temporary directory is {1}.", nl,
                                   Environment.GetEnvironmentVariable("TEMP"));

            Console.WriteLine("GetEnvironmentVariables: ");
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();
            foreach (DictionaryEntry de in environmentVariables)
            {
                Console.WriteLine("  {0} = {1}", de.Key, de.Value);
            }

            Console.WriteLine("GetFolderPath: {0}",
                         Environment.GetFolderPath(Environment.SpecialFolder.System));

            string[] drives = Environment.GetLogicalDrives();
            Console.WriteLine("GetLogicalDrives: {0}", String.Join(", ", drives));
        }

        public string SQLEnbelezado(string query)
        {
            string queryTratada = query.ToUpper();


            queryTratada = queryTratada.Replace("SELECT", $"{Environment.NewLine}SELECT");
            queryTratada = queryTratada.Replace("WHEN", $"{Environment.NewLine}WHEN");
            queryTratada = queryTratada.Replace("THEN", $"{Environment.NewLine}THEN");
            queryTratada = queryTratada.Replace("FROM", $"{Environment.NewLine}FROM");

            queryTratada = queryTratada.Replace("AND", $"{Environment.NewLine}AND");
            queryTratada = queryTratada.Replace("@", $"{Environment.NewLine}@");
            queryTratada = queryTratada.Replace("INNER", $"{Environment.NewLine}INNER");
            queryTratada = queryTratada.Replace("LEFT", $"{Environment.NewLine}LEFT");
            queryTratada = queryTratada.Replace("GROUP BY", $"{Environment.NewLine}GROUP BY{Environment.NewLine} ");
            queryTratada = queryTratada.Replace("WHERE", $"{Environment.NewLine}WHERE{Environment.NewLine} ");
           
            queryTratada = queryTratada.Replace($"AND{Environment.NewLine}@", $"@");
             queryTratada = queryTratada.Replace($"= {Environment.NewLine}@", $"= @");
            
            queryTratada = queryTratada.Replace(",", $"{Environment.NewLine},");
            queryTratada = queryTratada.Replace($",{Environment.NewLine}@", $", @");
            queryTratada = queryTratada.Replace($"'{Environment.NewLine}@", $"'{Environment.NewLine}  @");


         


            return queryTratada;
        }

        private void BTNEditar2_Click_1(object sender, EventArgs e)
        {
            DesativaCampos(true);
            LBStatus.Text = "Pronto para editar";
        }
        public void DesativaCampos(Boolean status)
        {
            if (status)
            {
                TXTInstancia.Enabled = true; TXTUsuario.Enabled = true; TXTUsuario.Enabled = true;
                TXTSenha.Enabled = true; TXTPastaData.Enabled = true; 
            }
            else
            {
                TXTInstancia.Enabled = false; TXTUsuario.Enabled = false; TXTUsuario.Enabled = false;
                TXTSenha.Enabled = false; TXTPastaData.Enabled = false; 
            }
        }
    }
}
