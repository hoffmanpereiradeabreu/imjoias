namespace Apresenta��o.Financeiro.Pagamento
{
    partial class CadastroOuro
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label4 = new System.Windows.Forms.Label();
            this.txtCota��o = new Apresenta��o.Mercadoria.Cota��o.TxtCota��o();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPeso = new AMS.TextBox.NumericTextBox();
            this.chkOuroMil = new System.Windows.Forms.RadioButton();
            this.chkOuroFundir = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic�cone)).BeginInit();
            this.SuspendLayout();
            // 
            // chkPagamentoPendente
            // 
            this.chkPagamentoPendente.Location = new System.Drawing.Point(2, 330);
            this.chkPagamentoPendente.TabIndex = 4;
            // 
            // txtValor
            // 
            this.txtValor.BackColor = System.Drawing.Color.LightGray;
            this.txtValor.ReadOnly = true;
            this.txtValor.Size = new System.Drawing.Size(267, 20);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkOuroFundir);
            this.groupBox1.Controls.Add(this.chkOuroMil);
            this.groupBox1.Controls.Add(this.txtPeso);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtCota��o);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Size = new System.Drawing.Size(276, 353);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.Controls.SetChildIndex(this.data, 0);
            this.groupBox1.Controls.SetChildIndex(this.txtDescri��o, 0);
            this.groupBox1.Controls.SetChildIndex(this.chkCobrarJuros, 0);
            this.groupBox1.Controls.SetChildIndex(this.txtValor, 0);
            this.groupBox1.Controls.SetChildIndex(this.label4, 0);
            this.groupBox1.Controls.SetChildIndex(this.chkPagamentoPendente, 0);
            this.groupBox1.Controls.SetChildIndex(this.txtCota��o, 0);
            this.groupBox1.Controls.SetChildIndex(this.label6, 0);
            this.groupBox1.Controls.SetChildIndex(this.txtPeso, 0);
            this.groupBox1.Controls.SetChildIndex(this.chkOuroMil, 0);
            this.groupBox1.Controls.SetChildIndex(this.chkOuroFundir, 0);
            // 
            // groupBox2
            // 
            this.groupBox2.Size = new System.Drawing.Size(192, 206);
            // 
            // chkCobrarJuros
            // 
            this.chkCobrarJuros.Location = new System.Drawing.Point(2, 307);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(124, 452);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(200, 452);
            // 
            // data
            // 
            this.data.Size = new System.Drawing.Size(264, 20);
            // 
            // txtDescri��o
            // 
            this.txtDescri��o.Size = new System.Drawing.Size(261, 91);
            // 
            // lblT�tulo
            // 
            this.lblT�tulo.Size = new System.Drawing.Size(48, 20);
            this.lblT�tulo.Text = "Ouro";
            // 
            // lblDescri��o
            // 
            this.lblDescri��o.Size = new System.Drawing.Size(192, 48);
            this.lblDescri��o.Text = "Fundir: valor = peso * cotacao da venda * 0.750 * 0.9.\r\nMil:" +
    " valor = peso * cotacao da venda.";
            // 
            // pic�cone
            // 
            this.pic�cone.Image = global::Apresenta��o.Resource.bot�o___ouro;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-1, 207);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Cota��o:";
            // 
            // txtCota��o
            // 
            this.txtCota��o.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCota��o.AvisarCota��esDesatualizadas = false;
            this.txtCota��o.AvisarCota��esN�oCadastradas = false;
            this.txtCota��o.AvisarNovaCota��o = false;
            this.txtCota��o.Cota��o = null;
            this.txtCota��o.IniciarValorAtual = false;
            this.txtCota��o.Location = new System.Drawing.Point(5, 223);
            this.txtCota��o.Name = "txtCota��o";
            this.txtCota��o.Size = new System.Drawing.Size(259, 20);
            this.txtCota��o.TabIndex = 19;
            this.txtCota��o.Valor = 0D;
            this.txtCota��o.Validated += new System.EventHandler(this.txtCota��o_Validated);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(-1, 246);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Peso:";
            // 
            // txtPeso
            // 
            this.txtPeso.AllowNegative = true;
            this.txtPeso.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPeso.DigitsInGroup = 0;
            this.txtPeso.Flags = 0;
            this.txtPeso.Location = new System.Drawing.Point(2, 260);
            this.txtPeso.MaxDecimalPlaces = 4;
            this.txtPeso.MaxWholeDigits = 9;
            this.txtPeso.Name = "txtPeso";
            this.txtPeso.Prefix = "";
            this.txtPeso.RangeMax = 1.7976931348623157E+308D;
            this.txtPeso.RangeMin = -1.7976931348623157E+308D;
            this.txtPeso.Size = new System.Drawing.Size(262, 20);
            this.txtPeso.TabIndex = 21;
            this.txtPeso.Text = "1";
            this.txtPeso.Validated += new System.EventHandler(this.txtPeso_Validated);
            // 
            // chkOuroMil
            // 
            this.chkOuroMil.AutoSize = true;
            this.chkOuroMil.Checked = true;
            this.chkOuroMil.Location = new System.Drawing.Point(2, 285);
            this.chkOuroMil.Name = "chkOuroMil";
            this.chkOuroMil.Size = new System.Drawing.Size(64, 17);
            this.chkOuroMil.TabIndex = 22;
            this.chkOuroMil.TabStop = true;
            this.chkOuroMil.Text = "Ouro Mil";
            this.chkOuroMil.UseVisualStyleBackColor = true;
            this.chkOuroMil.CheckedChanged += new System.EventHandler(this.chkOuroMil_CheckedChanged);
            // 
            // chkOuroFundir
            // 
            this.chkOuroFundir.AutoSize = true;
            this.chkOuroFundir.Location = new System.Drawing.Point(96, 285);
            this.chkOuroFundir.Name = "chkOuroFundir";
            this.chkOuroFundir.Size = new System.Drawing.Size(101, 17);
            this.chkOuroFundir.TabIndex = 23;
            this.chkOuroFundir.Text = "Ouro para fundir";
            this.chkOuroFundir.UseVisualStyleBackColor = true;
            this.chkOuroFundir.CheckedChanged += new System.EventHandler(this.chkOuroMil_CheckedChanged);
            // 
            // CadastroOuro
            // 
            this.ClientSize = new System.Drawing.Size(280, 487);
            this.Name = "CadastroOuro";
            this.Text = "Ouro";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic�cone)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton chkOuroFundir;
        private System.Windows.Forms.RadioButton chkOuroMil;
        private AMS.TextBox.NumericTextBox txtPeso;
        private System.Windows.Forms.Label label6;
        private Apresenta��o.Mercadoria.Cota��o.TxtCota��o txtCota��o;
    }
}
