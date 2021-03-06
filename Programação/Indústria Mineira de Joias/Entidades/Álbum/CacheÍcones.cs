﻿using Acesso.Comum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Entidades.Álbum
{
    public class CacheÍcones : DbManipulaçãoSimples
    {
        private Encoding codificador = Encoding.Default;

        private static CacheÍcones instância;

        public static CacheÍcones Instância
        {
            get
            {
                if (instância == null)
                    instância = new CacheÍcones();

                return instância;
            }
        }

        /// <summary>
        /// Dado uma chave GerarChave(), retorna o ícone.
        /// </summary>
        private Dictionary<string, Ícone> hashÍcones;

        /// <summary>
        /// Obtem a miniatura de uma foto. 
        /// Este método deve ser evitado, é preferível utilizar 
        /// ObterMiniaturas() para obtenção em batch com mais desempenho.
        /// </summary>
        /// <param name="código"></param>
        /// <returns></returns>
        public Ícone Obter(Entidades.Mercadoria.Mercadoria mercadoriaFoto)
        {
            Ícone entidade;

            if (hashÍcones.TryGetValue(GerarChave(mercadoriaFoto.ReferênciaNumérica), out entidade))
                return entidade;
            else
                return null;
        }

        private CacheÍcones()
        {
            Carregar();
        }


        private static string GerarChave(string referenciaNuméria)
        {
            return referenciaNuméria;
        }

        /// <summary>
        /// Faz uma consulta ao BD
        /// Carregando todos os ícones para a memória.
        /// </summary>
        public void Carregar()
        {
            DateTime inicio = DateTime.Now;
            Console.WriteLine("Consulta de obtenção de ícones!");

            hashÍcones = new Dictionary<string, Ícone>(StringComparer.Ordinal);
            string consulta = "SELECT mercadoria, icone FROM foto where icone is not null";

            IDbConnection conexão = Conexão;

            lock (conexão)
            {
                Usuários.UsuárioAtual.GerenciadorConexões.RemoverConexão(conexão);


                using (IDbCommand cmd = conexão.CreateCommand())
                {
                    cmd.CommandText = consulta;

                    IDataReader leitor = null;

                    try
                    {
                        using (leitor = cmd.ExecuteReader())
                        {

                            while (leitor.Read())
                            {
                                string chave;
                                chave = GerarChave(leitor.GetString(0));
                                hashÍcones[chave] = new Ícone((byte[])leitor.GetValue(1));
                            }
                        }
                    }
                    finally
                    {
                        if (leitor != null && !leitor.IsClosed)
                            leitor.Close();

                        Usuários.UsuárioAtual.GerenciadorConexões.AdicionarConexão(conexão);
                    }
                }
            }

            TimeSpan tempo = DateTime.Now - inicio;
            Console.WriteLine("total carga ícones: " + tempo.ToString());
        }
    }
}
