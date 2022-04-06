namespace RestaurarBancoProj
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.SERVIDOR = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CBServidor = new System.Windows.Forms.ComboBox();
            this.CBBancoDados = new System.Windows.Forms.ComboBox();
            this.TBUsuario = new System.Windows.Forms.TextBox();
            this.TBSenha = new System.Windows.Forms.TextBox();
            this.BTNAlterar = new System.Windows.Forms.Button();
            this.BTNAlterar2 = new System.Windows.Forms.Button();
            this.CBSegunracaIntegrada = new System.Windows.Forms.CheckBox();
            this.BTNBackup = new System.Windows.Forms.Button();
            this.BTNRestore = new System.Windows.Forms.Button();
            this.BTNEncerrar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SERVIDOR
            // 
            this.SERVIDOR.AutoSize = true;
            this.SERVIDOR.Location = new System.Drawing.Point(58, 20);
            this.SERVIDOR.Name = "SERVIDOR";
            this.SERVIDOR.Size = new System.Drawing.Size(63, 13);
            this.SERVIDOR.TabIndex = 0;
            this.SERVIDOR.Text = "SERVIDOR";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(65, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "USUARIO";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(77, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "SENHA";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "BANCO DE DADOS";
            // 
            // CBServidor
            // 
            this.CBServidor.FormattingEnabled = true;
            this.CBServidor.Location = new System.Drawing.Point(127, 12);
            this.CBServidor.Name = "CBServidor";
            this.CBServidor.Size = new System.Drawing.Size(121, 21);
            this.CBServidor.TabIndex = 4;
            this.CBServidor.SelectedIndexChanged += new System.EventHandler(this.CBServidor_SelectedIndexChanged);
            // 
            // CBBancoDados
            // 
            this.CBBancoDados.FormattingEnabled = true;
            this.CBBancoDados.Location = new System.Drawing.Point(127, 88);
            this.CBBancoDados.Name = "CBBancoDados";
            this.CBBancoDados.Size = new System.Drawing.Size(121, 21);
            this.CBBancoDados.TabIndex = 5;
            this.CBBancoDados.SelectedIndexChanged += new System.EventHandler(this.CBBancoDados_SelectedIndexChanged);
            this.CBBancoDados.Click += new System.EventHandler(this.CBBancoDados_Click);
            // 
            // TBUsuario
            // 
            this.TBUsuario.Location = new System.Drawing.Point(127, 36);
            this.TBUsuario.Name = "TBUsuario";
            this.TBUsuario.Size = new System.Drawing.Size(121, 20);
            this.TBUsuario.TabIndex = 6;
            this.TBUsuario.TextChanged += new System.EventHandler(this.TBUsuario_TextChanged);
            // 
            // TBSenha
            // 
            this.TBSenha.Location = new System.Drawing.Point(127, 62);
            this.TBSenha.Name = "TBSenha";
            this.TBSenha.Size = new System.Drawing.Size(121, 20);
            this.TBSenha.TabIndex = 7;
            // 
            // BTNAlterar
            // 
            this.BTNAlterar.Location = new System.Drawing.Point(254, 12);
            this.BTNAlterar.Name = "BTNAlterar";
            this.BTNAlterar.Size = new System.Drawing.Size(75, 23);
            this.BTNAlterar.TabIndex = 8;
            this.BTNAlterar.Text = "Alterar";
            this.BTNAlterar.UseVisualStyleBackColor = true;
            this.BTNAlterar.Click += new System.EventHandler(this.BTNAlterar_Click);
            // 
            // BTNAlterar2
            // 
            this.BTNAlterar2.Location = new System.Drawing.Point(254, 86);
            this.BTNAlterar2.Name = "BTNAlterar2";
            this.BTNAlterar2.Size = new System.Drawing.Size(75, 23);
            this.BTNAlterar2.TabIndex = 9;
            this.BTNAlterar2.Text = "Alterar";
            this.BTNAlterar2.UseVisualStyleBackColor = true;
            this.BTNAlterar2.Click += new System.EventHandler(this.BTNAlterar2_Click);
            // 
            // CBSegunracaIntegrada
            // 
            this.CBSegunracaIntegrada.AutoSize = true;
            this.CBSegunracaIntegrada.Location = new System.Drawing.Point(127, 125);
            this.CBSegunracaIntegrada.Name = "CBSegunracaIntegrada";
            this.CBSegunracaIntegrada.Size = new System.Drawing.Size(115, 17);
            this.CBSegunracaIntegrada.TabIndex = 10;
            this.CBSegunracaIntegrada.Text = "Integrated Security";
            this.CBSegunracaIntegrada.UseVisualStyleBackColor = true;
            this.CBSegunracaIntegrada.CheckedChanged += new System.EventHandler(this.CBSegunracaIntegrada_CheckedChanged);
            // 
            // BTNBackup
            // 
            this.BTNBackup.Location = new System.Drawing.Point(92, 169);
            this.BTNBackup.Name = "BTNBackup";
            this.BTNBackup.Size = new System.Drawing.Size(75, 23);
            this.BTNBackup.TabIndex = 11;
            this.BTNBackup.Text = "Backup";
            this.BTNBackup.UseVisualStyleBackColor = true;
            this.BTNBackup.Click += new System.EventHandler(this.BTNBackup_Click);
            // 
            // BTNRestore
            // 
            this.BTNRestore.Location = new System.Drawing.Point(173, 169);
            this.BTNRestore.Name = "BTNRestore";
            this.BTNRestore.Size = new System.Drawing.Size(75, 23);
            this.BTNRestore.TabIndex = 12;
            this.BTNRestore.Text = "Restore";
            this.BTNRestore.UseVisualStyleBackColor = true;
            this.BTNRestore.Click += new System.EventHandler(this.BTNRestore_Click);
            // 
            // BTNEncerrar
            // 
            this.BTNEncerrar.Location = new System.Drawing.Point(254, 169);
            this.BTNEncerrar.Name = "BTNEncerrar";
            this.BTNEncerrar.Size = new System.Drawing.Size(75, 23);
            this.BTNEncerrar.TabIndex = 13;
            this.BTNEncerrar.Text = "Encerrar";
            this.BTNEncerrar.UseVisualStyleBackColor = true;
            this.BTNEncerrar.Click += new System.EventHandler(this.BTNEncerrar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 230);
            this.Controls.Add(this.BTNEncerrar);
            this.Controls.Add(this.BTNRestore);
            this.Controls.Add(this.BTNBackup);
            this.Controls.Add(this.CBSegunracaIntegrada);
            this.Controls.Add(this.BTNAlterar2);
            this.Controls.Add(this.BTNAlterar);
            this.Controls.Add(this.TBSenha);
            this.Controls.Add(this.TBUsuario);
            this.Controls.Add(this.CBBancoDados);
            this.Controls.Add(this.CBServidor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SERVIDOR);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SERVIDOR;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CBServidor;
        private System.Windows.Forms.ComboBox CBBancoDados;
        private System.Windows.Forms.TextBox TBUsuario;
        private System.Windows.Forms.TextBox TBSenha;
        private System.Windows.Forms.Button BTNAlterar;
        private System.Windows.Forms.Button BTNAlterar2;
        private System.Windows.Forms.CheckBox CBSegunracaIntegrada;
        private System.Windows.Forms.Button BTNBackup;
        private System.Windows.Forms.Button BTNRestore;
        private System.Windows.Forms.Button BTNEncerrar;
    }
}

