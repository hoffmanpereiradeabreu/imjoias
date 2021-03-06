using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Entidades.�lbum;
using Apresenta��o.Formul�rios;
using Apresenta��o.�lbum.Edi��o.Impress�o;
using Apresenta��o.�lbum.Edi��o.�lbuns.Desenhista;
using Apresenta��o.�lbum.Edi��o.Fotos;

namespace Apresenta��o.�lbum.Edi��o.�lbuns
{
    public partial class BaseEditar�lbum : Apresenta��o.Formul�rios.BaseInferior
    {
        private ListaFotos listaFotosTodas;

        public BaseEditar�lbum()
        {
            InitializeComponent();

            lstEdi��o.BaseInferior = this;

            listaFotosTodas = todasFotos.ListaFotosTodas;
            listaFotosTodas.MouseMove += listaFotosTodas_MouseMove;
        }

        public BaseEditar�lbum(Entidades.�lbum.�lbum �lbum)
            : this()
        {
            lstEdi��o.�lbum = �lbum;
            lstEdi��o.BaseInferior = this;

            t�tuloBaseInferior.Descri��o = �lbum.Nome + "\n" + �lbum.Descri��o;
        }

        /// <summary>
        /// Ocorre ao exibir a base inferior.
        /// </summary>
        protected override void AoExibir()
        {
            base.AoExibir();

            todasFotos.Carregar();

            // Talvez que o vinculo foto-album tenha sido alterado:
            Entidades.�lbum.�lbum album = Entidades.�lbum.�lbum.Obter�lbum(lstEdi��o.�lbum.C�digo);
            lstEdi��o.Carregar(album);
        }

        private bool dragging = false;

        /// <summary>
        /// Ocorre ao pressionar o mouse na lista de todas as fotos.
        /// </summary>
        private void listaFotosTodas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging && e.Button == MouseButtons.Left)
                try
                {
                    Foto[] fotos = listaFotosTodas.Sele��es;

                    if (fotos.Length > 0)
                    {
                        dragging = false;
                        listaFotosTodas.DoDragDrop(listaFotosTodas.Sele��es, DragDropEffects.Link);
                    }
                }
                catch (Exception erro)
                {
                    Acesso.Comum.Usu�rios.Usu�rioAtual.RegistrarErro(erro);

#if DEBUG
                    MessageBox.Show("N�o foi poss�vel iniciar drag'n'drop!\n\n" + erro.ToString());
#endif
                }
        }

        private void listaFotosTodas_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = false;
        }



        /// <summary>
        /// Exclui o �lbum inteiro.
        /// </summary>
        private void op��oExcluir_Click(object sender, EventArgs e)
        {
            bool confirmado;
            Entidades.�lbum.�lbum �lbum;

            �lbum = lstEdi��o.�lbum;

            if (�lbum.Fotos.ContarElementos() == 0)
                confirmado = true;
            else
            {
                confirmado = MessageBox.Show(
                    this.ParentForm,
                    "Deseja mesmo excluir o �lbum " + �lbum.Nome + "?\n\n"
                    + "(Obs.: As fotos n�o ser�o excluidas do banco de dados.",
                    "Excluir �lbum",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes;

                if (confirmado)
                    confirmado = MessageBox.Show(
                        this.ParentForm,
                        "Voc� tem certeza?\n\nN�O SER� POSS�VEL REVERTER ESSA A��O!",
                        "Excluir �lbum",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2) == DialogResult.Yes;
            }

            if (confirmado)
            {
                AguardeDB.Mostrar();
                �lbum.Descadastrar();
                AguardeDB.Fechar();
                SubstituirBaseParaInicial();
            }
        }

        /// <summary>
        /// Imprime o �lbum.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void op��oImprimir_Click(object sender, EventArgs e)
        {
            using (JanelaOp��esImpress�o dlg = new JanelaOp��esImpress�o())
            {
                if (dlg.ShowDialog(ParentForm) == DialogResult.OK)
                    ControleImpress�o.Imprimir(lstEdi��o.�lbum, dlg.Itens);
            }
        }

        /// <summary>
        /// Renomea o �lbum.
        /// </summary>
        private void op��oRenomear_Click(object sender, EventArgs e)
        {
            using (Cadastro�lbum dlg = new Cadastro�lbum(lstEdi��o.�lbum))
            {
                if (dlg.ShowDialog(this.ParentForm) == DialogResult.OK)
                    t�tuloBaseInferior.Descri��o = lstEdi��o.�lbum.Nome + "\n" + lstEdi��o.�lbum.Descri��o;
            }
        }

        private void lnkMaisFotos_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            BaseTodasFotos novaBase = new BaseTodasFotos();
            SubstituirBase(novaBase);
        }

        private void listaFotosTodas_AoExclu�do(Foto foto)
        {
            Entidades.�lbum.�lbum album = Entidades.�lbum.�lbum.Obter�lbum(lstEdi��o.�lbum.C�digo);
            lstEdi��o.Carregar(album);

            todasFotos.Carregar();
        }
    }
}

