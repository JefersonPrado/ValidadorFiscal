using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestaurarBancoProj

{

    public partial class Form1 : Form
    {
        //enumera uma lista de instâncias locais disponíveis do SQL Server
        DataTable tabelaServidores = SmoApplication.EnumAvailableSqlServers(true);
        //define o objeto do tipo Server
        private static Server servidor;
        //define o caminho para o backup/restore (pasta bin/Debug)
        private string DBpath = Application.StartupPath;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            WindowState = FormWindowState.Normal;
            CBServidor.Enabled = false;
            CBBancoDados.Enabled = false;
            try
            {
                CBSegunracaIntegrada.Checked = true;
                // Se existerem servidores
                if (tabelaServidores.Rows.Count > 0)
                {
                    // Percorre cada servidor na tabela
                    foreach (DataRow drServer in tabelaServidores.Rows)
                    {
                        CBServidor.Items.Add(drServer["Name"]);
                        CBServidor.Text = Convert.ToString(drServer["Name"]);
                    }
                }
            }
            catch (Exception)
            {
                // Inicie o serviço do SQL Server Browser se não conseguir carregar os servidores.(http://msdn.microsoft.com/en-us/library/ms165734(v=sql.90).aspx
                MessageBox.Show("ERROR: Não existem servidores disponíveis.\nOu ocorreu um erro ao carregar os servidores", "Servidor Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            this.Cursor = Cursors.Default;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }




        private void CBServidor_SelectedIndexChanged(object sender, EventArgs e)
        {
            CBServidor.Enabled = false;
        }

        private void CBBancoDados_SelectedIndexChanged(object sender, EventArgs e)
        {
            CBBancoDados.Enabled = false;
        }

        private void BTNAlterar2_Click(object sender, EventArgs e)
        {
            BTNAlterar2.Enabled = false;
        }

        private void BTNAlterar_Click(object sender, EventArgs e)
        {
            BTNAlterar.Enabled = false;
        }

        private void BTNBackup_Click(object sender, EventArgs e)
        {
            //verifica se um banco de dados foi selecionado
            if (CBBancoDados.SelectedIndex.ToString().Equals(""))
            {
                MessageBox.Show("Selecione um Database", "Server", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //se o objeto servidor for diferente de null temos uma conexão
            if (servidor != null)
            {
                try
                {
                    //desabilita os botões
                    BTNBackup.Enabled = false;
                    BTNRestore.Enabled = false;
                    BTNAlterar2.Enabled = false;
                    BTNAlterar.Enabled = false;
                    //Este codigo é usado se você já criou o arquivo de backup.
                    File.Delete(DBpath + "\\backup.bak");
                    this.Cursor = Cursors.WaitCursor;
                    // se o usuário escolheu um caminho onde salvar o backup
                    // Cria uma nova operação de backup
                    Backup bkpDatabase = new Backup();
                    // Define o tipo de backup type para o database
                    bkpDatabase.Action = BackupActionType.Database;
                    // Define o database que desejamos fazer o backup
                    bkpDatabase.Database = CBBancoDados.SelectedItem.ToString();
                    // Define o dispositivo do backup para : file
                    BackupDeviceItem bkpDevice = new BackupDeviceItem(DBpath + "\\Backup.bak", DeviceType.File);
                    // Adiciona o dispositivo de backup ao backup
                    bkpDatabase.Devices.Add(bkpDevice);
                    // Realiza o backup
                    bkpDatabase.SqlBackup(servidor);
                    MessageBox.Show("Backup do Database " + CBBancoDados.Text + " criado com sucesso", "Servidor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception x)
                {
                    MessageBox.Show("ERRO: Ocorreu um erro durante o BACKUP do DataBase" + x, "Erro no Servidor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Cursor = Cursors.Arrow;
                    //habilita os botões
                    BTNBackup.Enabled = true;
                    BTNRestore.Enabled = true;
                    BTNAlterar.Enabled = true;
                    BTNAlterar2.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("ERRO: Não foi estabelecida uma conexão com o SQL Server", "Servidor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Arrow;
            }
        }

        private void BTNRestore_Click(object sender, EventArgs e)
        {

            //verifica se foi selecoinado um banco de dados
            if (CBBancoDados.SelectedIndex.ToString().Equals(""))
            {
                MessageBox.Show("Escolha um banco de dados", "Servidor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // Se existir um conexão SQL Server criada
            if (servidor != null)
            {
                try
                {
                    //desabilita os botões
                    BTNBackup.Enabled = false;
                    BTNRestore.Enabled = false;
                    BTNAlterar2.Enabled = false;
                    BTNAlterar.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;
                    // Se foi escolhido o arquivo o arquivo que deseja ser restaurado
                    // Cria uma nova operação de restore
                    Restore rstDatabase = new Restore();
                    // Define o tipo de restore para o banco de dados
                    rstDatabase.Action = RestoreActionType.Database;
                    // Define o database que desejamos restaurar
                    rstDatabase.Database = CBBancoDados.SelectedItem.ToString();
                    // Define o dispostivo de backup a partir do qual vamos restaurar o arquivo
                    BackupDeviceItem bkpDevice = new BackupDeviceItem(DBpath + "\\Backup.bak", DeviceType.File);
                    // Adiciona o dispositivo de backup ao tipo de restore
                    rstDatabase.Devices.Add(bkpDevice);
                    // Se o banco de dados ja existe então subsititui
                    rstDatabase.ReplaceDatabase = true;
                    // Realiza o Restore
                    rstDatabase.SqlRestore(servidor);
                    MessageBox.Show("Database " + CBBancoDados.Text + " RESTAURADO com sucesso", "Servidor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("ERRO: Ocorreu um erro durante a restauração do banco de dados", "Erro na aplicação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Cursor = Cursors.Arrow;
                    //habilita os botões
                    BTNBackup.Enabled = true;
                    BTNRestore.Enabled = true;
                    BTNAlterar.Enabled = true;
                    BTNAlterar2.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("ERRO: Não foi estabelecida uma conexão com o SQL Server", "Servidor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Arrow;
            }
        }

        private void BTNEncerrar_Click(object sender, EventArgs e)
        {

        }

        private void CBSegunracaIntegrada_CheckedChanged(object sender, EventArgs e)
        {

            //verifica se o checkbox esta marcado ou desmarcado e altera as propriedades dos TextBox
            if (CBSegunracaIntegrada.CheckState == CheckState.Checked)
            {
                TBUsuario.Enabled = false;
                TBUsuario.Text = string.Empty;
                TBSenha.Enabled = false;
                TBSenha.Text = string.Empty;
            }
            if (CBSegunracaIntegrada.CheckState == CheckState.Unchecked)
            {
                TBUsuario.Enabled = true;
                TBSenha.Enabled = true;
            }
        }

        private void TBUsuario_TextChanged(object sender, EventArgs e)
        {

        }

        private void CBBancoDados_Click(object sender, EventArgs e)
        {

            //limpa o combobox dos databases
            CBBancoDados.Items.Clear();
            try
            {
                //se foi selecionado um servidor
                if (CBServidor.SelectedItem != null && CBServidor.SelectedItem.ToString() != "")
                {
                    this.Cursor = Cursors.WaitCursor;
                    // Cria uma nova conexão com o servidor selecionado
                    ServerConnection srvConn = new ServerConnection(CBServidor.SelectedItem.ToString());
                    // Faz o Login usando a autenticacao SQL ao invés da autenticação do Windows
                    srvConn.LoginSecure = true;
                    //tipo de conexão não exige usuário e senha(usa a autenticação do windows)
                    if (CBSegunracaIntegrada.CheckState == CheckState.Checked)
                    {
                        // Cria um novo objeto SQL Server usando a conexão criada
                        servidor = new Server(srvConn);
                        // percorre a lista de banco de dados
                        foreach (Database dbServer in servidor.Databases)
                        {
                            // Adiciona o banco de dados na combobox
                            CBBancoDados.Items.Add(dbServer.Name);
                        }
                    }
                    //tipo de conexão exige usuário e senha
                    if (CBSegunracaIntegrada.CheckState == CheckState.Unchecked)
                    {
                        // atribui o nome do usuário
                        srvConn.Login = TBUsuario.Text;
                        // atribui a senha
                        srvConn.Password = TBSenha.Text;
                        // Cria um novo objeto SQL Server usando a conexão criada
                        servidor = new Server(srvConn);
                        // percorre a lista de banco de dados
                        foreach (Database dbServer in servidor.Databases)
                        {
                            // Adiciona o banco de dados na combobox
                            CBBancoDados.Items.Add(dbServer.Name);
                        }
                    }
                }
                else
                {
                    // Um servidor não foi selecionado exibe um erro
                    MessageBox.Show("ERROR: Contate o Administrador!!", "Servidor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                // Inicie o serviço do SQL Server Browser se não conseguir carregar os servidores.(http://msdn.microsoft.com/en-us/library/ms165734(v=sql.90).aspx
                MessageBox.Show("ERROR: Ocorreu um erro durante a carga dos banco de dados disponíveis", "Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }

        }
    }
}
