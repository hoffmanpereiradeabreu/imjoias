﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades.Mercadoria
{
    /// <summary>
    /// Coeficientes de uma mercadoria.
    /// </summary>
    public class Coeficientes
    {
        private Dictionary<uint, double> hashTabela;

        public Coeficientes()
        {
            hashTabela = new Dictionary<uint, double>();
        }

        internal void AdicionarCoeficiente(uint tabela, double valor)
        {
            if (hashTabela.ContainsKey(tabela))
            {
                throw new Exception("Tentativa de adicionar um indice ja cadastrado, para tabela: " + tabela.ToString() + " valor já existente: " + hashTabela[tabela].ToString() + " valor a ser colocado: " + valor.ToString());
            }

            hashTabela[tabela] = valor;
        }

        public double this[Tabela tabela]
        {
            get
            {
                if (tabela == null)
                    throw new ArgumentNullException("Tabela não pode ser nula!");

                return hashTabela[tabela.Código];
            }
        }
    }
}
