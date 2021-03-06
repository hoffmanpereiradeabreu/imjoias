using Entidades.Privil�gio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Apresenta��o.Formul�rios
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class BaseFormul�rio : System.Windows.Forms.Form
	{
		// Atributos
		private BaseInferior baseAtual;

		/// <summary>
		/// Delega��o para evento de substitui��o. O par�metro cancelar
		/// impede a substitui��o da base inferior.
		/// </summary>
		public delegate void Substitui��o(object sender, BaseFormul�rio formul�rio, BaseInferior anterior, BaseInferior nova, out bool cancelar);

		/// <summary>
		/// Ocorre ao substituir a base inferior.
		/// </summary>
		public event Substitui��o AoSubstituirBaseInferior;

		// Formul�rio
		protected Panel topo;
		private Panel conte�do;
		protected BaseInferior baseInferior;
		protected BarraBot�es barraBot�es;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public BaseFormul�rio()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			baseAtual = baseInferior;

            if (Screen.PrimaryScreen.Bounds.Width <= 800)
            {
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }
		}

        /// <summary>
        /// Ocorre toda vez que um assembly � carregado tardiamente.
        /// </summary>
        private void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            foreach (ExporBot�oAttribute exporta��o in 
                args.LoadedAssembly.GetCustomAttributes(typeof(ExporBot�oAttribute), true))
            {
                Bot�o bot�o = ExportarBot�o(args.LoadedAssembly, exporta��o);

                if (bot�o != null)
                    bot�o.Controlador.AoCarregarCompletamente(null);
            }
        }

        private static int ordem = 1;

        /// <summary>
        /// Insere o bot�o exportado � barra de bot�es.
        /// </summary>
        private Bot�o ExportarBot�o(Assembly assembly, ExporBot�oAttribute exporta��o)
        {
            if (exporta��o.ExigirFuncion�rio && Entidades.Pessoa.Funcion�rio.Funcion�rioAtual == null)
                return null;

            if (Permiss�oFuncion�rio.ValidarPermiss�o(exporta��o.Permiss�es))
            {
                Bot�o bot�o;

                if (exporta��o.Bot�o == null)
                {
                    try
                    {
                        bot�o = new Bot�o();
                        bot�o.Texto = exporta��o.Texto;
                        bot�o.Privil�gios = exporta��o.Permiss�es;
                        bot�o.BaseFormul�rio = this;

                        if (exporta��o.Ordena��o.HasValue)
                            bot�o.Ordena��o = exporta��o.Ordena��o.Value;
                        else
                        {
                            bot�o.Ordena��o = ordem;

                            if (ordem > 0)
                                ordem = -ordem - 1;
                            else
                                ordem = -ordem + 1;
                        }

                        if (exporta��o.Controlador != null)
                            bot�o.Controlador = (ControladorBaseInferior)exporta��o.Controlador.Assembly.
                                CreateInstance(exporta��o.Controlador.FullName);

                        bot�o.Retornar�Primeira = exporta��o.Retornar�Primeira;

                        foreach (Type tipo in exporta��o.Bases)
                        {
                            BaseInferior novaBase = (BaseInferior)tipo.Assembly.CreateInstance(tipo.FullName);
                            bot�o.Controlador.InserirBaseInferior(novaBase);
                        }

                        if (bot�o.Controlador.Bases.Count > 0 && bot�o.Controlador.Bases[0].Imagem != null)
                            bot�o.Imagem = bot�o.Controlador.Bases[0].Imagem;
                    }
                    catch (Exce��oBot�oN�oSuportado)
                    {
                        return null;
                    }
                }
                else
                    bot�o = (Bot�o)exporta��o.Bot�o.Assembly.CreateInstance(exporta��o.Bot�o.FullName);

                barraBot�es.AdicionarBot�o(bot�o);

                return bot�o;
            }

            return null;
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );

            barraBot�es.Dispose();
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseFormul�rio));
            this.topo = new System.Windows.Forms.Panel();
            this.barraBot�es = new Apresenta��o.Formul�rios.BarraBot�es();
            this.conte�do = new System.Windows.Forms.Panel();
            this.baseInferior = new Apresenta��o.Formul�rios.BaseInferior();
            this.topo.SuspendLayout();
            this.conte�do.SuspendLayout();
            this.SuspendLayout();
            // 
            // topo
            // 
            this.topo.BackColor = System.Drawing.Color.Transparent;
            this.topo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("topo.BackgroundImage")));
            this.topo.Controls.Add(this.barraBot�es);
            this.topo.Dock = System.Windows.Forms.DockStyle.Top;
            this.topo.Location = new System.Drawing.Point(0, 0);
            this.topo.Name = "topo";
            this.topo.Size = new System.Drawing.Size(1200, 93);
            this.topo.TabIndex = 2;
            // 
            // barraBot�es
            // 
            this.barraBot�es.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.barraBot�es.BackColor = System.Drawing.Color.Transparent;
            this.barraBot�es.Bot�es = new Apresenta��o.Formul�rios.Bot�o[0];
            this.barraBot�es.Cursor = System.Windows.Forms.Cursors.Hand;
            this.barraBot�es.Location = new System.Drawing.Point(188, 0);
            this.barraBot�es.Name = "barraBot�es";
            this.barraBot�es.Size = new System.Drawing.Size(1012, 75);
            this.barraBot�es.TabIndex = 0;
            // 
            // conte�do
            // 
            this.conte�do.Controls.Add(this.baseInferior);
            this.conte�do.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conte�do.Location = new System.Drawing.Point(0, 93);
            this.conte�do.Name = "conte�do";
            this.conte�do.Size = new System.Drawing.Size(1200, 490);
            this.conte�do.TabIndex = 3;
            // 
            // baseInferior
            // 
            this.baseInferior.BackColor = System.Drawing.Color.White;
            this.baseInferior.Dock = System.Windows.Forms.DockStyle.Fill;
            this.baseInferior.Location = new System.Drawing.Point(0, 0);
            this.baseInferior.Name = "baseInferior";
            this.baseInferior.Size = new System.Drawing.Size(1200, 490);
            this.baseInferior.TabIndex = 4;
            // 
            // BaseFormul�rio
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1200, 583);
            this.Controls.Add(this.conte�do);
            this.Controls.Add(this.topo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "BaseFormul�rio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ind�stria Mineira de Joias";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.topo.ResumeLayout(false);
            this.conte�do.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Substitui base inferior por outra.
		/// </summary>
		protected internal void SubstituirBase(BaseInferior b, object sender)
		{
            UseWaitCursor = true;

            try
            {
                if (b == null)
                    b = baseInferior;

                // Dispara evento de substitui��o.
                if (AoSubstituirBaseInferior != null)
                {
                    bool cancelar;

                    AoSubstituirBaseInferior(sender, this, baseAtual, b, out cancelar);

                    if (cancelar)
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                // Verifica se controladora � a mesma
                if (b.Controlador != baseAtual.Controlador && baseAtual.Controlador != null)
                    baseAtual.Controlador.AoEsconder();

                this.SuspendLayout();
                conte�do.SuspendLayout();

                // Preparar nova base inferior.
                b.SuspendLayout();
                b.Dock = DockStyle.None;
                b.Size = conte�do.Size;
                b.Location = new System.Drawing.Point(0, 0);
                b.ResumeLayout();

                conte�do.Controls.Clear();
                conte�do.Controls.Add(b);

                conte�do.ResumeLayout();
                this.ResumeLayout();

                b.Dock = DockStyle.Fill;

                baseAtual.DispararAoSerSubstituido(b);

                // Verifica se controladora � a mesma
                if (b.Controlador != baseAtual.Controlador && b.Controlador != null)
                    b.Controlador.AoExibir();

                baseAtual = b;

                b.DispararAoExibir();
            }
            finally
            {
                UseWaitCursor = false;
            }
		}

		/// <summary>
		/// Prepara base para ser utilizada
		/// </summary>
		/// <param name="b">Base inferior a ser utilizada.</param>
		protected internal void NovaBase(BaseInferior b)
		{
			b.BackColor = baseInferior.BackColor;
			b.Dock = DockStyle.Fill;
			b.Name = "baseInferior";
			b.Location = new System.Drawing.Point(0, 93);
			b.Size = new System.Drawing.Size(792, 413);
			b.TabIndex = 2;
		}

		/// <summary>
		/// � chamada assim que a todo o programa j� est� carregado.
		/// </summary>
        protected virtual void AoCarregarCompletamente(Splash splash)
        {
            bool falha = false;
            string problema = "";
            List<Bot�o> remo��o = new List<Bot�o>();

            foreach (Bot�o bot�o in barraBot�es.Bot�es)
            {
                bot�o.Controlador.AoCarregarCompletamente(splash);
            }

            foreach (Bot�o bot�o in remo��o)
            {
                barraBot�es.RemoverBot�o(bot�o);
                bot�o.Dispose();
            }

            baseInferior.AoCarregarCompletamente(splash);
            baseInferior.DispararAoExibir();

            if (falha)
                throw new Exce��oP�sCarga(problema);
        }

		/// <summary>
		/// Deve ser chamado pelo programa de apresenta��o depois
        /// que os bot�es s�o configurados.
		/// </summary>
        /// 
		public void DispararAoCarregar(Splash splash)
		{
			Cursor cursorAnterior = Cursor.Current;

			Cursor.Current = Cursors.WaitCursor;

            if (splash != null)
                splash.Mensagem = "Carregando mercadorias ...";

            Entidades.Mercadoria.Mercadoria.IniciarCarga();

            ImportarBot�es(splash);

            // Dispara o evento
			AoCarregarCompletamente(splash);

            this.Text = "Ind�stria Mineira de Joias " + Vers�o.Descri��o;

#if DEBUG
            this.Text += " @ " + Acesso.MySQL.MySQLUsu�rios.Host;
#endif
            new Entidades.Configura��o.Configura��oUsu�rio<string>("Vers�oT�tular", Vers�o.Descri��o).Valor = Vers�o.Descri��o;

            this.Refresh();
			
			Cursor.Current = cursorAnterior;
		}

        private void ImportarBot�es(Splash splash)
        {
            try
            {
                string caminho = AppDomain.CurrentDomain.BaseDirectory;

                if (splash != null)
                    splash.Mensagem = "Carregando controles gr�ficos ...";

                string arquivo = Path.Combine(caminho, "Apresenta��o.dll");

                    Assembly assembly = Assembly.LoadFile(arquivo);

                    foreach (ExporBot�oAttribute exporta��o in assembly.GetCustomAttributes(typeof(ExporBot�oAttribute), true))
                        ExportarBot�o(assembly, exporta��o);

                AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(CurrentDomain_AssemblyLoad);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                Application.Exit();
            }
        }

		/// <summary>
		/// Base atual.
		/// </summary>
		public BaseInferior BaseAtual
		{
			get { return baseAtual; }
		}

        protected override void OnClosed(EventArgs e)
        {
            baseAtual.DispararAoSerSubstituido(null);

            base.OnClosed(e);

            Dispose();
        }
	}
}
