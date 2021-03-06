using Acesso.Comum;
using Entidades.Acerto;
using Entidades.Balan�o;
using System;
using System.Collections.Generic;
using System.Data;

namespace Entidades.Relacionamento.Sa�da
{
    [Serializable, DbTabela("saida")]
    public class Sa�da : RelacionamentoAcerto
    {
        [DbRelacionamento("codigo", "pessoa")]
        protected Pessoa.Pessoa pessoa;

        [DbColuna("cotacao")]
        protected double cota��o;

        #region Propriedades

        /// <summary>
        /// Cota��o no momento da sa�da.
        /// </summary>
        public double Cota��o
        {
            get { return cota��o; }
            set { cota��o = value; DefinirDesatualizado(); }
        }

        public new Hist�ricoRelacionamentoSa�da Itens
        {
            get
            {
                return (Hist�ricoRelacionamentoSa�da) base.Itens;
            }
        }

        public override Entidades.Pessoa.Pessoa Pessoa
        {
            get { return pessoa; }
            set
            {
                DefinirDesatualizado();
                pessoa = value;
            }
        }

        #endregion

        protected override Hist�ricoRelacionamento ConstruirItens()
        {
            return new Hist�ricoRelacionamentoSa�da(this);
        }

        public Sa�da() { }

        public Sa�da(Entidades.Pessoa.Pessoa pessoa)
        {
            this.pessoa = pessoa;
        }

        public bool ObterTravadoEmCache()
        {
            return travado;
        }

        public override bool Travado
        {
            get
            {
                if (!Cadastrado) return travado;

                IDbConnection conex�o = Conex�o;

                lock (conex�o)
                {
                    using (IDbCommand cmd = conex�o.CreateCommand())
                    {
                        cmd.CommandText =
                            "SELECT travado FROM saida where codigo=" + DbTransformar(codigo);

                        travado = Convert.ToBoolean(cmd.ExecuteScalar());
                    }
                }

                return travado;
            }
            set
            {
                travado = value;

                DefinirDesatualizado();

                if (Cadastrado)
                    Atualizar();
            }
        }

        private enum Ordem
        {
            IndiceItem, DataItem, Funcion�rioItem, C�digo, Refer�ncia, PesoSa�da, Quantidade,
            Referencia, Nome, Teor, Peso, Faixa, Grupo, Digito, ForaDeLinha, DePeso
        }

        public static List<Sa�da> ObterSa�das(AcertoConsignado acerto)
        {
            List<Sa�da> sa�das = Mapear<Sa�da>(
                "SELECT * FROM saida WHERE acerto = " + DbTransformar(acerto.C�digo));

            return sa�das;
        }

        public static List<Sa�da> ObterSa�das(Pessoa.Pessoa pessoa, DateTime in�cio, DateTime final, bool apenasN�oAcertados)
        {
            return ObterSa�dasSemItens(pessoa, in�cio, final, apenasN�oAcertados);
        }

        private static List<Sa�da> ObterSa�dasSemItens(Pessoa.Pessoa pessoa, DateTime in�cio, DateTime final, bool apenasN�oAcertados)
        {
            IDbConnection conex�o;
            List<Sa�da> sa�das;

            conex�o = Conex�o;

            lock (conex�o)
            {
                using (IDbCommand cmd = conex�o.CreateCommand())
                {
                    cmd.CommandText = "SELECT * from saida where 1=1 ";

                    if (apenasN�oAcertados)
                        cmd.CommandText += " AND acerto in (select codigo from acertoconsignado where dataefetiva is null) ";

                    if (pessoa != null)
                        cmd.CommandText += " AND pessoa=" + pessoa.C�digo;

                    cmd.CommandText +=
                    " AND data BETWEEN " + DbTransformar((DateTime?)in�cio) + " AND " + DbTransformar(final);

                    sa�das = Mapear<Sa�da>(cmd);
                }
            }

            return sa�das;
        }


        public static uint ContarSa�dasN�oAcertadas(Pessoa.Pessoa pessoa)
        {
            IDbConnection conex�o;

            conex�o = Conex�o;

            lock (conex�o)
                using (IDbCommand cmd = conex�o.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM saida WHERE acerto in (select codigo from acertoconsignado where dataefetiva is null) AND pessoa="
                        + DbTransformar(pessoa.C�digo);

                    return Convert.ToUInt32(cmd.ExecuteScalar());
                }
        }

        private enum OrdemAcerto { Refer�ncia, D�gito, Peso, Quantidade, �ndice };

        private static void ObterAcerto(string consulta, Dictionary<string, Acerto.SaquinhoAcerto> hash, F�rmulaAcerto f�rmula)
        {
            IDbConnection conex�o;
            IDataReader leitor = null;

            conex�o = Conex�o;

            using (IDbCommand cmd = conex�o.CreateCommand())
            {
                cmd.CommandText = consulta;

                lock (conex�o)
                {
                    Usu�rios.Usu�rioAtual.GerenciadorConex�es.RemoverConex�o(conex�o);
                    try
                    {
                        using (leitor = cmd.ExecuteReader())
                        {

                            while (leitor.Read())
                            {
                                string refer�ncia = leitor.GetString((int)OrdemAcerto.Refer�ncia);
                                byte d�gito = leitor.GetByte((int)OrdemAcerto.D�gito);
                                double qtd = leitor.GetDouble((int)OrdemAcerto.Quantidade);
                                double peso = leitor.GetDouble((int)OrdemAcerto.Peso);
                                double �ndice = leitor.GetDouble((int)OrdemAcerto.�ndice);

                                SaquinhoAcerto itemNovo = SaquinhoAcerto.Construir(f�rmula, new Mercadoria.Mercadoria(refer�ncia, d�gito, peso, �ndice), 0, peso, �ndice);

                                bool itemJ�Existente;

                                SaquinhoAcerto item;

                                Mercadoria.Mercadoria mercadoria = new Mercadoria.Mercadoria(refer�ncia, d�gito, peso, null);
                                itemJ�Existente = hash.TryGetValue(itemNovo.Identifica��oAgrup�vel(), out item);

                                // Primeira vez deste item: utiliza um novinho
                                if (!itemJ�Existente)
                                    item = itemNovo;

                                item.QtdSa�da += qtd;

                                if (!itemJ�Existente)
                                    hash.Add(item.Identifica��oAgrup�vel(), item);
                            }
                        }
                    }
                    finally
                    {
                        if (leitor != null)
                            leitor.Close();

                        Usu�rios.Usu�rioAtual.GerenciadorConex�es.AdicionarConex�o(conex�o);
                    }

                }
            }
        }

        private static void ObterAcerto(string consulta, Dictionary<string, SaquinhoBalan�o> hash)
        {
            IDbConnection conex�o;
            IDataReader leitor = null;

            conex�o = Conex�o;

            using (IDbCommand cmd = conex�o.CreateCommand())
            {
                cmd.CommandText = consulta;

                lock (conex�o)
                {
                    Usu�rios.Usu�rioAtual.GerenciadorConex�es.RemoverConex�o(conex�o);
                    try
                    {
                        using (leitor = cmd.ExecuteReader())
                        {
                            while (leitor.Read())
                            {
                                string refer�ncia = leitor.GetString((int)OrdemAcerto.Refer�ncia);
                                byte d�gito = leitor.GetByte((int)OrdemAcerto.D�gito);
                                double qtd = leitor.GetDouble((int)OrdemAcerto.Quantidade);
                                double peso = leitor.GetDouble((int)OrdemAcerto.Peso);
                                double �ndice = leitor.GetDouble((int)OrdemAcerto.�ndice);

                                SaquinhoBalan�o itemNovo = new SaquinhoBalan�o(new Mercadoria.Mercadoria(refer�ncia, d�gito, peso, �ndice), 0, peso, �ndice);

                                bool itemJ�Existente;

                                SaquinhoBalan�o item;

                                Mercadoria.Mercadoria mercadoria = new Mercadoria.Mercadoria(refer�ncia, d�gito, peso, null);
                                itemJ�Existente = hash.TryGetValue(itemNovo.Identifica��oAgrup�vel(), out item);

                                // Primeira vez deste item: utiliza um novinho
                                if (!itemJ�Existente)
                                    item = itemNovo;

                                item.QtdSa�da += qtd;

                                if (!itemJ�Existente)
                                    hash.Add(item.Identifica��oAgrup�vel(), item);
                            }
                        }
                    }
                    finally
                    {
                        if (leitor != null)
                            leitor.Close();

                        Usu�rios.Usu�rioAtual.GerenciadorConex�es.AdicionarConex�o(conex�o);
                    }

                }
            }
        }


        public static List<long> ObterAcerto(List<long> c�digoSa�das, Dictionary<string, Acerto.SaquinhoAcerto> hash, F�rmulaAcerto f�rmula)
        {
            string consulta;
            
            if (c�digoSa�das.Count != 0) 
            {
                consulta = "select saidaitem.referencia, mercadoria.digito, saidaitem.peso, sum(quantidade) as qtd, saidaitem.indice"
                    + " from saidaitem, saida, mercadoria WHERE saida.codigo=saidaitem.saida AND saida.codigo IN "
                    + DbTransformarConjunto(c�digoSa�das)
                    + " AND mercadoria.referencia=saidaitem.referencia group by referencia, digito, peso, indice having qtd != 0 order by referencia, peso";

                ObterAcerto(consulta, hash, f�rmula);
            }

            return c�digoSa�das;
        }

        private enum OrdemRastro { Data, Documento, Quantidade };

        public static void PreencherRastro(Entidades.Mercadoria.Mercadoria mercadoria, Pessoa.Pessoa pessoa, List<RastroItem> lista, List<long> c�digoSa�das)
        {
            IDbConnection conex�o;
            IDataReader leitor = null;

            if (c�digoSa�das.Count == 0)
                return;

            conex�o = Conex�o;

            using (IDbCommand cmd = conex�o.CreateCommand())
            {
                lock (conex�o)
                {
                    Usu�rios.Usu�rioAtual.GerenciadorConex�es.RemoverConex�o(conex�o);

                    cmd.CommandText = "select saida.data, saida.codigo, sum(saidaitem.quantidade) as qtd from saida "
                    + ", saidaitem WHERE saidaitem.saida=saida.codigo "
                    + " AND referencia=" + DbTransformar(mercadoria.Refer�nciaNum�rica);

                    if (mercadoria.DePeso)
                        cmd.CommandText += " AND peso=" + DbTransformar(mercadoria.Peso);

                    cmd.CommandText += " AND saida.codigo IN " + DbTransformarConjunto(c�digoSa�das);
                    cmd.CommandText += " GROUP BY saida.codigo, referencia, peso HAVING qtd != 0 ORDER by data";

                    try
                    {
                        using (leitor = cmd.ExecuteReader())
                        {
                            while (leitor.Read())
                            {
                                DateTime data = leitor.GetDateTime((int)OrdemRastro.Data);
                                long documento = leitor.GetInt64((int)OrdemRastro.Documento);
                                double qtd = leitor.GetDouble((int)OrdemRastro.Quantidade);

                                RastroItem rastro = new RastroItem(RastroItem.TipoEnum.Sa�da, data, documento, qtd);
                                rastro.Documento = "Sa�da #" + documento.ToString();
                                lista.Add(rastro);
                            }
                        }
                    }
                    finally
                    {
                        if (leitor != null)
                            leitor.Close();

                        Usu�rios.Usu�rioAtual.GerenciadorConex�es.AdicionarConex�o(conex�o);
                    }

                }
            }
        }

        public static Sa�da ObterSa�da(long c�digo)
        {
            return ObterSa�da((ulong)c�digo);
        }

        public static Sa�da ObterSa�da(ulong c�digo)
        {
            Sa�da sa�da;
            
            sa�da = Mapear�nicaLinha<Sa�da>("SELECT * FROM saida WHERE "
                    + "codigo = " + DbTransformar(c�digo));

            return sa�da;
        }

        /// <summary>
        /// Obt�m a data da �ltima sa�da.
        /// </summary>
        /// <returns>Data da �ltima sa�da.</returns>
        public static DateTime ObterData�ltimaSa�da(Entidades.Pessoa.Pessoa pessoa)
        {
            IDbConnection conex�o = Conex�o;

            lock (conex�o)
                using (IDbCommand cmd = conex�o.CreateCommand())
                {
                    cmd.CommandText = "SELECT MAX(data) FROM saida WHERE pessoa = " + DbTransformar(pessoa.C�digo);

                    try
                    {
                        return Convert.ToDateTime(cmd.ExecuteScalar());
                    }
                    catch
                    {
                        return DateTime.MinValue;
                    }
                }
        }

        /// <summary>
        /// Obt�m a data da �ltima sa�da acertada.
        /// </summary>
        /// <returns>Data da �ltima sa�da.</returns>
        public static DateTime ObterData�ltimaSa�daAcertada(Entidades.Pessoa.Pessoa pessoa)
        {
            IDbConnection conex�o = Conex�o;

            lock (conex�o)
                using (IDbCommand cmd = conex�o.CreateCommand())
                {
                    cmd.CommandText = "SELECT MAX(data) FROM saida WHERE pessoa = "
                        + DbTransformar(pessoa.C�digo)
                        + " AND acerto in (select codigo from acertoconsignado where dataefetiva is not null) ";

                    try
                    {
                        return Convert.ToDateTime(cmd.ExecuteScalar());
                    }
                    catch
                    {
                        return DateTime.MinValue;
                    }
                }
        }

        /// <summary>
        /// Obt�m a data da primeira sa�da n�o acertada.
        /// </summary>
        /// <returns>Data da primeira sa�da n�o acertada.</returns>
        public static DateTime ObterDataPrimeiraSa�daN�oAcertada(Entidades.Pessoa.Pessoa pessoa)
        {
            IDbConnection conex�o = Conex�o;

            lock (conex�o)
                using (IDbCommand cmd = conex�o.CreateCommand())
                {
                    cmd.CommandText = "SELECT MIN(data) FROM saida WHERE pessoa = "
                        + DbTransformar(pessoa.C�digo)
                        + " AND acerto in (select codigo from acertoconsignado where dataefetiva is null) ";

                    try
                    {
                        return Convert.ToDateTime(cmd.ExecuteScalar());
                    }
                    catch
                    {
                        return DateTime.MinValue;
                    }
                }
        }

        public static void TravarV�rios(List<long> c�digos)
        {
            IDbConnection conex�o = Conex�o;
           
             if (c�digos.Count == 0) return;

            lock (conex�o)
                using (IDbCommand cmd = conex�o.CreateCommand())
                {
                    cmd.CommandText = "update saida set travado=1 ";
                    cmd.CommandText += " where codigo IN " + DbTransformarConjunto(c�digos);
                    cmd.ExecuteNonQuery();
                }

        }

        public override string ToString()
        {
            return "Sa�da " + C�digo.ToString();
        }

        protected override void Cadastrar(IDbCommand cmd)
        {
            System.Diagnostics.Debug.Assert(cota��o > 0);
            System.Diagnostics.Debug.Assert(tabela != null);

            base.Cadastrar(cmd);
        }

        public static void ObterAcerto(List<long> sa�das, Dictionary<string, Balan�o.SaquinhoBalan�o> hash)
        {
            string consulta;

            if (sa�das.Count != 0)
            {
                consulta = "select saidaitem.referencia, mercadoria.digito, saidaitem.peso, sum(quantidade) as qtd, saidaitem.indice"
                    + " from saidaitem, saida, mercadoria WHERE saida.codigo=saidaitem.saida AND saida.codigo IN "
                    + DbTransformarConjunto(sa�das)
                    + " AND mercadoria.referencia=saidaitem.referencia group by referencia, digito, peso, indice having qtd != 0 order by referencia, peso";

                ObterAcerto(consulta, hash);
            }

            return;
        }
    }
}
