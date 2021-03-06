
namespace Apresentação.Administrativo
{
    partial class BaseAdministrativa
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseAdministrativa));
            this.títuloBaseInferior = new Apresentação.Formulários.TítuloBaseInferior();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.quadroOpçãoImportação = new Apresentação.Formulários.QuadroOpção();
            this.quadroExportação = new Apresentação.Formulários.QuadroOpção();
            this.quadroExportacaoCupom = new Apresentação.Formulários.QuadroOpção();
            this.quadroExportaVenda = new Apresentação.Formulários.QuadroOpção();
            this.quadroOpçãoBalanço = new Apresentação.Formulários.QuadroOpção();
            this.quadroComissão = new Apresentação.Formulários.QuadroOpção();
            this.quadroControleEstoque = new Apresentação.Formulários.QuadroOpção();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // esquerda
            // 
            this.esquerda.Size = new System.Drawing.Size(187, 631);
            // 
            // títuloBaseInferior
            // 
            this.títuloBaseInferior.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.títuloBaseInferior.BackColor = System.Drawing.Color.White;
            this.títuloBaseInferior.Descrição = "Escolha uma tarefa administrativa.";
            this.títuloBaseInferior.ÍconeArredondado = false;
            this.títuloBaseInferior.Imagem = global::Apresentação.Resource.gravata1;
            this.títuloBaseInferior.Location = new System.Drawing.Point(207, 12);
            this.títuloBaseInferior.Name = "títuloBaseInferior";
            this.títuloBaseInferior.Size = new System.Drawing.Size(883, 70);
            this.títuloBaseInferior.TabIndex = 6;
            this.títuloBaseInferior.Título = "Administrativo";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.quadroOpçãoImportação);
            this.flowLayoutPanel1.Controls.Add(this.quadroExportação);
            this.flowLayoutPanel1.Controls.Add(this.quadroExportacaoCupom);
            this.flowLayoutPanel1.Controls.Add(this.quadroExportaVenda);
            this.flowLayoutPanel1.Controls.Add(this.quadroOpçãoBalanço);
            this.flowLayoutPanel1.Controls.Add(this.quadroComissão);
            this.flowLayoutPanel1.Controls.Add(this.quadroControleEstoque);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(207, 103);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(10);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(904, 495);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // quadroOpçãoImportação
            // 
            this.quadroOpçãoImportação.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("quadroOpçãoImportação.BackgroundImage")));
            this.quadroOpçãoImportação.Cursor = System.Windows.Forms.Cursors.Hand;
            this.quadroOpçãoImportação.Descrição = "Importa preços do sistema legado. É necessário indicar a pasta dos arquivos cadme" +
    "r.dbf, ccusto.dbf e gramas.dbf";
            this.quadroOpçãoImportação.Ícone = ((System.Drawing.Image)(resources.GetObject("quadroOpçãoImportação.Ícone")));
            this.quadroOpçãoImportação.Location = new System.Drawing.Point(3, 3);
            this.quadroOpçãoImportação.MaximumSize = new System.Drawing.Size(600, 70);
            this.quadroOpçãoImportação.MinimumSize = new System.Drawing.Size(200, 70);
            this.quadroOpçãoImportação.Name = "quadroOpçãoImportação";
            this.quadroOpçãoImportação.Size = new System.Drawing.Size(295, 70);
            this.quadroOpçãoImportação.TabIndex = 1;
            this.quadroOpçãoImportação.Título = "Integração";
            this.quadroOpçãoImportação.Click += new System.EventHandler(this.quadroOpçãoImportação_Click);
            // 
            // quadroExportação
            // 
            this.quadroExportação.Cursor = System.Windows.Forms.Cursors.Hand;
            this.quadroExportação.Descrição = "Gerador de arquivos para PDV formato antigo (BR-500)";
            this.quadroExportação.Ícone = ((System.Drawing.Image)(resources.GetObject("quadroExportação.Ícone")));
            this.quadroExportação.Location = new System.Drawing.Point(3, 79);
            this.quadroExportação.MaximumSize = new System.Drawing.Size(600, 70);
            this.quadroExportação.MinimumSize = new System.Drawing.Size(200, 70);
            this.quadroExportação.Name = "quadroExportação";
            this.quadroExportação.Size = new System.Drawing.Size(295, 70);
            this.quadroExportação.TabIndex = 2;
            this.quadroExportação.Título = "PDV BR-500";
            this.quadroExportação.Click += new System.EventHandler(this.quadroExportação_Click);
            // 
            // quadroExportacaoCupom
            // 
            this.quadroExportacaoCupom.Cursor = System.Windows.Forms.Cursors.Hand;
            this.quadroExportacaoCupom.Descrição = "Gerador de arquivos para PDV formato e-connect.";
            this.quadroExportacaoCupom.Ícone = ((System.Drawing.Image)(resources.GetObject("quadroExportacaoCupom.Ícone")));
            this.quadroExportacaoCupom.Location = new System.Drawing.Point(3, 155);
            this.quadroExportacaoCupom.MaximumSize = new System.Drawing.Size(600, 70);
            this.quadroExportacaoCupom.MinimumSize = new System.Drawing.Size(200, 70);
            this.quadroExportacaoCupom.Name = "quadroExportacaoCupom";
            this.quadroExportacaoCupom.Size = new System.Drawing.Size(295, 70);
            this.quadroExportacaoCupom.TabIndex = 4;
            this.quadroExportacaoCupom.Título = "PDV e-connect";
            this.quadroExportacaoCupom.Click += new System.EventHandler(this.quadroExportacaoCupom_Click);
            // 
            // quadroExportaVenda
            // 
            this.quadroExportaVenda.Cursor = System.Windows.Forms.Cursors.Hand;
            this.quadroExportaVenda.Descrição = "Gerador de nota fiscal eletrônica";
            this.quadroExportaVenda.Ícone = ((System.Drawing.Image)(resources.GetObject("quadroExportaVenda.Ícone")));
            this.quadroExportaVenda.Location = new System.Drawing.Point(3, 231);
            this.quadroExportaVenda.MaximumSize = new System.Drawing.Size(600, 70);
            this.quadroExportaVenda.MinimumSize = new System.Drawing.Size(200, 70);
            this.quadroExportaVenda.Name = "quadroExportaVenda";
            this.quadroExportaVenda.Size = new System.Drawing.Size(295, 70);
            this.quadroExportaVenda.TabIndex = 5;
            this.quadroExportaVenda.Título = "Gerador NF-e";
            this.quadroExportaVenda.Click += new System.EventHandler(this.quadroExportaVenda_Click);
            // 
            // quadroOpçãoBalanço
            // 
            this.quadroOpçãoBalanço.Cursor = System.Windows.Forms.Cursors.Hand;
            this.quadroOpçãoBalanço.Descrição = "Exibe relatório com o resumo de mercadorias de consignado.";
            this.quadroOpçãoBalanço.Ícone = ((System.Drawing.Image)(resources.GetObject("quadroOpçãoBalanço.Ícone")));
            this.quadroOpçãoBalanço.Location = new System.Drawing.Point(3, 307);
            this.quadroOpçãoBalanço.MaximumSize = new System.Drawing.Size(600, 70);
            this.quadroOpçãoBalanço.MinimumSize = new System.Drawing.Size(200, 70);
            this.quadroOpçãoBalanço.Name = "quadroOpçãoBalanço";
            this.quadroOpçãoBalanço.Size = new System.Drawing.Size(295, 70);
            this.quadroOpçãoBalanço.TabIndex = 0;
            this.quadroOpçãoBalanço.Título = "Balanço";
            this.quadroOpçãoBalanço.Click += new System.EventHandler(this.quadroOpçãoBalanço_Click);
            // 
            // quadroComissão
            // 
            this.quadroComissão.Cursor = System.Windows.Forms.Cursors.Hand;
            this.quadroComissão.Descrição = "Cálculo de comissão";
            this.quadroComissão.Ícone = global::Apresentação.Resource.comissao;
            this.quadroComissão.Location = new System.Drawing.Point(3, 383);
            this.quadroComissão.MaximumSize = new System.Drawing.Size(600, 70);
            this.quadroComissão.MinimumSize = new System.Drawing.Size(200, 70);
            this.quadroComissão.Name = "quadroComissão";
            this.quadroComissão.Size = new System.Drawing.Size(295, 70);
            this.quadroComissão.TabIndex = 3;
            this.quadroComissão.Título = "Comissão";
            this.quadroComissão.Click += new System.EventHandler(this.quadroComissão_Click);
            // 
            // quadroControleEstoque
            // 
            this.quadroControleEstoque.Cursor = System.Windows.Forms.Cursors.Hand;
            this.quadroControleEstoque.Descrição = "Controle de estoque";
            this.quadroControleEstoque.Ícone = ((System.Drawing.Image)(resources.GetObject("quadroControleEstoque.Ícone")));
            this.quadroControleEstoque.Location = new System.Drawing.Point(304, 3);
            this.quadroControleEstoque.MaximumSize = new System.Drawing.Size(600, 70);
            this.quadroControleEstoque.MinimumSize = new System.Drawing.Size(200, 70);
            this.quadroControleEstoque.Name = "quadroControleEstoque";
            this.quadroControleEstoque.Size = new System.Drawing.Size(295, 70);
            this.quadroControleEstoque.TabIndex = 6;
            this.quadroControleEstoque.Título = "Estoque";
            this.quadroControleEstoque.Click += new System.EventHandler(this.quadroControleEstoque_Click);
            // 
            // BaseAdministrativa
            // 
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.títuloBaseInferior);
            this.Imagem = global::Apresentação.Resource.gravata1;
            this.Name = "BaseAdministrativa";
            this.Size = new System.Drawing.Size(1114, 631);
            this.Controls.SetChildIndex(this.títuloBaseInferior, 0);
            this.Controls.SetChildIndex(this.flowLayoutPanel1, 0);
            this.Controls.SetChildIndex(this.esquerda, 0);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Apresentação.Formulários.TítuloBaseInferior títuloBaseInferior;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Apresentação.Formulários.QuadroOpção quadroOpçãoBalanço;
        private Apresentação.Formulários.QuadroOpção quadroOpçãoImportação;
        private Apresentação.Formulários.QuadroOpção quadroExportação;
        private Apresentação.Formulários.QuadroOpção quadroComissão;
        private Formulários.QuadroOpção quadroExportacaoCupom;
        private Formulários.QuadroOpção quadroExportaVenda;
        private Formulários.QuadroOpção quadroControleEstoque;
    }
}
