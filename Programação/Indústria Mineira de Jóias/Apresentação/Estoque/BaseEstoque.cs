﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Apresentação.Formulários;
using Entidades.Estoque;
using Apresentação.Estoque.Extrato;

namespace Apresentação.Estoque
{
    public partial class BaseEstoque : BaseInferior
    {
        private Saldo saldo;

        public BaseEstoque()
        {
            InitializeComponent();
        }

        private void opçãoZerarEstoque_Click(object sender, EventArgs e)
        {
            SubstituirBase(new BaseZerarEstoque());
        }

        private void btnExtrato_Click(object sender, EventArgs e)
        {
            if (listaSaldo.Seleção == null)
            {
                MessageBox.Show(this,
                    "Selecionar uma referência primeiro",
                    "Nada selecionado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
            else
                AbrirExtrato(listaSaldo.Seleção);
        }

        private void btnEntradas_Click(object sender, EventArgs e)
        {
            SubstituirBase(new Entrada.BaseEntradas());
        }

        protected override void AoExibir()
        {
            base.AoExibir();

            listaSaldo.Carregar();
        }

        private void listaSaldo_AoDuploClique(object sender, EventArgs e)
        {
            AbrirExtrato(listaSaldo.Seleção);
        }

        private void AbrirExtrato(Saldo s)
        {
            this.saldo = s;

            BaseExtrato novaBase = new BaseExtrato();
            novaBase.Carregar(s);

            SubstituirBase(novaBase);
        }

        private void btnRelatórioResumo_Click(object sender, EventArgs e)
        {
            JanelaImpressão janela = new JanelaImpressão();

            Apresentação.Impressão.Relatórios.Estoque.ResumoSaldo.ResumoRelatorioSaldo r = 
                new Impressão.Relatórios.Estoque.ResumoSaldo.ResumoRelatorioSaldo();

            new Apresentação.Impressão.Relatórios.Estoque.ResumoSaldo.ControleImpressãoFornecedor().
                PrepararImpressão(r, listaSaldo.Itens);

            janela.Título = "Resumo";
            janela.Descrição = "Relatório de resumo de estoque";
            janela.InserirDocumento(r, "Estoque");

            janela.Show();
        }

        private void btnRelatórioFornecedor_Click(object sender, EventArgs e)
        {
            JanelaImpressão janela = new JanelaImpressão();

            Apresentação.Impressão.Relatórios.Estoque.Fornecedor.ResumoFornecedor r =
                new Impressão.Relatórios.Estoque.Fornecedor.ResumoFornecedor();

            new Apresentação.Impressão.Relatórios.Estoque.Fornecedor.ControleImpressãoFornecedor().
                PrepararImpressão(r, listaSaldo.Itens);

            janela.Título = "Resumo";
            janela.Descrição = "Relatório de resumo de estoque";
            janela.InserirDocumento(r, "Estoque");

            janela.Show();
        }
    }
}
