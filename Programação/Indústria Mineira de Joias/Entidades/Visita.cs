﻿using System;
using System.Collections.Generic;
using System.Text;
using Acesso.Comum;
using Entidades.Pessoa;
using System.Data;
using System.Collections;
using Entidades.Configuração;

namespace Entidades
{
    /// <summary>
    /// Registro de visita.
    /// </summary>
    [DbTransação]
    public class Visita : DbManipulaçãoAutomática
    {
        #region Atributos

        /// <summary>
        /// Momento em que o visitante entrou na empresa.
        /// </summary>
        [DbChavePrimária(false)]
        private DateTime entrada;

        /// <summary>
        /// Momento em que o visitante deixou a empresa.
        /// </summary>
        [DbColuna("saida")]
        private DateTime? saída;

        /// <summary>
        /// Tempo em segundos de espera por atendimento.
        /// </summary>
        private uint? espera;

        /// <summary>
        /// Motivo da visita do cliente.
        /// </summary>
        private MotivoContato motivo;

        /// <summary>
        /// Setor de atendimento.
        /// </summary>
        [DbRelacionamento("codigo", "setor")]
        private Setor setor;

        /// <summary>
        /// Lista de nomes dos visitantes não cadastrados.
        /// </summary>
        private DbComposiçãoInalterável<string> nomes;

        /// <summary>
        /// Lista de visitantes cadastrados.
        /// </summary>
        private DbComposiçãoInalterável<Pessoa.Pessoa> pessoas;

        /// <summary>
        /// Atendente da visita.
        /// </summary>
        [DbColuna("funcionario"), DbRelacionamento("codigo", "funcionario")]
        private Funcionário atendente;

        [DbAtributo(TipoAtributo.Ignorar)]
        private bool atendimentoForaDoRodízio = false;

        #endregion

        #region Propriedades

        public bool AtendimentoForaDoRodízio
        {
            get { return atendimentoForaDoRodízio; }
            set { atendimentoForaDoRodízio = value; }
        }

        /// <summary>
        /// Momento em que o visitante entrou na empresa.
        /// </summary>
        /// <remarks>
        /// Por ser chave primária, só pode ser alterado se
        /// o objeto não estiver cadastrado no banco de dados.
        /// </remarks>
        public DateTime Entrada
        {
            get { return entrada; }
            set
            {
                if (Cadastrado)
                    throw new Acesso.Comum.Exceções.AlteraçãoChavePrimária(this);

                entrada = value;
            }
        }

        /// <summary>
        /// Momento em que o visitante deixou a empresa.
        /// </summary>
        public DateTime? Saída
        {
            get { return saída; }
            set { saída = value; DefinirDesatualizado(); }
        }

        /// <summary>
        /// Tempo em segundos de espera por atendimento.
        /// </summary>
        public uint? Espera
        {
            get { return espera; }
            set { espera = value; DefinirDesatualizado(); }
        }

        /// <summary>
        /// Motivo da visita do cliente.
        /// </summary>
        private MotivoContato Motivo
        {
            get { return motivo; }
            set { motivo = value; DefinirDesatualizado(); }
        }

        /// <summary>
        /// Setor de atendimento.
        /// </summary>
        public Setor Setor
        {
            get { return setor; }
            set { setor = value; DefinirDesatualizado(); }
        }

        /// <summary>
        /// Lista de nomes dos visitantes não cadastrados.
        /// </summary>
        public DbComposiçãoInalterável<string> Nomes
        {
            get { return nomes; }
        }

        /// <summary>
        /// Lista de visitantes cadastrados.
        /// </summary>
        public DbComposiçãoInalterável<Pessoa.Pessoa> Pessoas
        {
            get { return pessoas; }
        }

        /// <summary>
        /// Atendente da visita.
        /// </summary>
        public Funcionário Atendente
        {
            get { return atendente; }
            set { atendente = value; DefinirDesatualizado(); }
        }

        #endregion

        /// <summary>
        /// Constrói a visita.
        /// </summary>
        public Visita()
        {
            nomes = new DbComposiçãoInalterável<string>(
                new DbAção<string>(CadastrarNome),
                new DbAção<string>(DescadastrarNome));

            pessoas = new DbComposiçãoInalterável<Pessoa.Pessoa>(
                new DbAção<Pessoa.Pessoa>(CadastrarPessoa),
                new DbAção<Pessoa.Pessoa>(DescadastrarPessoa));
        }

        /// <summary>
        /// Constrói a visita considerando já um cliente para atendimento.
        /// </summary>
        /// <param name="clientes">Clientes a serem atendidos.</param>
        public Visita(params PessoaFísica[] clientes)
            : this()
        {
            foreach (PessoaFísica cliente in clientes)
                pessoas.Adicionar(cliente);
        }

        #region Manipulação de nomes de visitantes.

        /// <summary>
        /// Cadastra um nome do visitante no banco de dados.
        /// </summary>
        private void CadastrarNome(IDbCommand cmd, string nome)
        {
            cmd.CommandText = "INSERT INTO visitanome (visita, nome) VALUES ("
                + DbTransformar(entrada) + ", "
                + DbTransformar(nome) + ")";

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Descadastra um nome do banco de dados.
        /// </summary>
        private void DescadastrarNome(IDbCommand cmd, string nome)
        {
            cmd.CommandText = "DELETE FROM visitanome WHERE "
                + "visita = " + DbTransformar(entrada) + " AND "
                + "nome = " + DbTransformar(nome);

            cmd.ExecuteNonQuery();
        }

        #endregion

        #region Manipulação de visitantes cadastrados.

        /// <summary>
        /// Cadastra a pessoa como visitante.
        /// </summary>
        private void CadastrarPessoa(IDbCommand cmd, Pessoa.Pessoa pessoaFísica)
        {
            cmd.CommandText = "INSERT INTO visitapessoafisica (visita, pessoafisica) VALUES ("
                + DbTransformar(entrada) + ","
                + DbTransformar(pessoaFísica.Código) + ")";

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Descadastra a pessoa como visitante.
        /// </summary>
        private void DescadastrarPessoa(IDbCommand cmd, Pessoa.Pessoa pessoaFísica)
        {
            cmd.CommandText = "DELETE FROM visitapessoafisica WHERE "
                + "entrada = " + DbTransformar(entrada) + " AND "
                + "pessoafisica = " + DbTransformar(pessoaFísica.Código);

            cmd.ExecuteNonQuery();
        }

        #endregion

        #region Recuperação do banco de dados

        /// <summary>
        /// Obtém próximo cliente a ser atendido por um setor.
        /// </summary>
        /// <param name="setor">Setor para atendimento.</param>
        /// <returns>Dados da visita do cliente que é o próximo a ser atendido.</returns>
        public static Visita ObterPróximaVisitaEsperando(Setor setor)
        {
            Visita visita;
            
            visita = MapearÚnicaLinha<Visita>("SELECT * FROM visita WHERE espera is NULL "
                        + "AND setor = " + DbTransformar(setor.Código)
                        + " ORDER BY entrada LIMIT 1");

            if (visita != null)
                CarregarRelacionamentos(new List<Visita>() { visita }, " v.entrada = " + DbTransformar(visita.Entrada));

            return visita;
        }

        /// <summary>
        /// Obtém clientes que visitaram a empresa em um determinado período.
        /// </summary>
        /// <param name="pInicial">Período inicial.</param>
        /// <param name="pFinal">Período final.</param>
        /// <returns>Visitas neste período.</returns>
        public static List<Visita> ObterVisitas(DateTime pInicial, DateTime pFinal)
        {
            /* Esta consulta pode ser otimizada,
             * porém sua complexidade será aumentada.
             * Como ela raramente é utilizada,
             * foi optado por deixá-la mais simples.
             * -- Júlio, 12/04/2006
             * 
             * Consulta otimizada para carga em apenas uma SQL. 
             * -- André, 19/04/2016.
             */

            List<Visita> visitas;

            string where = " v.entrada BETWEEN " + DbTransformar(pInicial) + " AND " + DbTransformar(pFinal);

            visitas = Mapear<Visita>("SELECT * FROM visita v WHERE " + where);

            CarregarRelacionamentos(visitas, where);

            return visitas;
        }

        /// <summary>
        /// Obtém clientes que visitaram ou sariam da empresa a partir
        /// de um determinado momento.
        /// </summary>
        /// <param name="pInicial">Período inicial.</param>
        /// <returns>Visitas neste período.</returns>
        public static List<Visita> ObterVisitas(DateTime pInicial)
        {
            List<Visita> visitas;

            string where = " v.entrada > " + DbTransformar(pInicial) +  " OR v.saida > " + DbTransformar(pInicial);
            visitas = Mapear<Visita>(
                "SELECT * FROM visita v WHERE " + where);

            CarregarRelacionamentos(visitas, where);

            return visitas;
        }

        private static Dictionary<DateTime, Visita> CriarHash(List<Visita> visitas)
        {
            Dictionary<DateTime, Visita> hash = new Dictionary<DateTime, Visita>();

            foreach (Visita v in visitas)
            {
                hash[v.entrada] = v;
            }

            return hash;
        }

   
        /// <summary>
        /// Carrega todos os relacionamentos da visita.
        /// </summary>
        private static void CarregarRelacionamentos(List<Visita> visitas, string where)
        {
            if (visitas.Count == 0)
                return;

            IDataReader leitor = null;
            StringBuilder str = new StringBuilder();
            Dictionary<DateTime, Visita> hash = CriarHash(visitas);
            Dictionary<DateTime, ulong> hashVisitaCódigo = new Dictionary<DateTime, ulong>();
            SortedSet<ulong> conjuntoPessoas = new SortedSet<ulong>();
 
            str.Append("select n.visita, n.nome, null as pessoaFisica from visitanome n JOIN visita v on n.visita=v.entrada where ");
            str.Append(where);
            str.Append(" UNION SELECT f.visita, null as nome, f.pessoaFisica from visitapessoafisica f JOIN visita v on f.visita=v.entrada where ");
            str.Append(where);

            string myStr = str.ToString();
            
            IDbConnection conexão = Conexão;
        
            lock (conexão)
            {
                Usuários.UsuárioAtual.GerenciadorConexões.RemoverConexão(conexão);

                try
                {
                    using (IDbCommand cmd = conexão.CreateCommand())
                    {
                        cmd.CommandText = str.ToString();

                        using (leitor = cmd.ExecuteReader())
                        {
                            while (leitor.Read())
                            {
                                Visita visita = hash[leitor.GetDateTime(0)];

                                if (!leitor.IsDBNull(1))
                                    visita.nomes.AdicionarJáCadastrado(leitor.GetString(1));

                                if (!leitor.IsDBNull(2))
                                {
                                    ulong código = Convert.ToUInt64(leitor.GetValue(2));
                                    hashVisitaCódigo.Add(visita.entrada, código);
                                    conjuntoPessoas.Add(código);
                                }
                            }
                        }
                    }
                } finally
                {
                    Usuários.UsuárioAtual.GerenciadorConexões.AdicionarConexão(conexão);

                    if (leitor != null)
                        leitor.Close();
                }
            }

            Dictionary<ulong, Entidades.Pessoa.Pessoa> hashPessoas = 
                Entidades.Pessoa.Pessoa.ObterPessoas(conjuntoPessoas);

            foreach(KeyValuePair<DateTime, Visita> par in hash)
            {
                DateTime entrada = par.Key;
                Visita visita = par.Value;
                ulong código;

                if (hashVisitaCódigo.TryGetValue(entrada, out código))
                {
                    Entidades.Pessoa.Pessoa pessoa = hashPessoas[código];
                    visita.Pessoas.AdicionarJáCadastrado(pessoa);
                }
            }
        }

        #endregion

        /// <summary>
        /// Verifica se um determinado funcionário está em atendimento.
        /// </summary>
        /// <param name="funcionário">Funcionário em atendimento.</param>
        /// <returns>Se o funcionário está em atendimento.</returns>
        public static bool VerificarAtendimento(Funcionário funcionário)
        {
            IDbConnection conexão = Conexão;
            int qtd;

            lock (conexão)
                using (IDbCommand cmd = conexão.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM visita v" +
                        " WHERE v.funcionario = " + DbTransformar(funcionário.Código) +
                        " AND v.saida IS NULL";

                    qtd = Convert.ToInt32(cmd.ExecuteScalar());
                }

            return qtd > 0;
        }

        /// <summary>
        /// Verifica se um determinado funcionário está em atendimento.
        /// </summary>
        /// <param name="funcionário">Funcionário em atendimento.</param>
        /// <returns>Se o funcionário está em atendimento.</returns>
        public static List<Visita> ObterAtendimentos(Funcionário funcionário)
        {
            string where = " v.funcionario = " + DbTransformar(funcionário.Código) + " AND v.saida IS NULL";

            List<Visita> visitas = Mapear<Visita>("SELECT v.* FROM visita v WHERE " + where);

            CarregarRelacionamentos(visitas, where);

            return visitas;
        }

        /// <summary>
        /// Obtém visitas relevantes conforme os critérios:
        /// (i) se está na empresa;
        /// (ii) se chegou à empresa no dia corrente.
        /// </summary>
        public static List<Visita> ObterVisitasRelevantes()
        {
            string where = " v.saida IS NULL OR v.entrada >= CURDATE() ";
            List<Visita> visitas = Mapear<Visita>("SELECT * FROM visita v WHERE " + 
                where + " ORDER BY entrada ");

            CarregarRelacionamentos(visitas,  where);

            return visitas;
        }

        /// <summary>
        /// Obtém visitas relevantes para um funcionário conforme os critérios:
        /// (i) se está na empresa;
        /// (ii) se chegou à empresa no dia corrente.
        /// </summary>
        public static List<Visita> ObterVisitasRelevantes(Funcionário funcionário, Setor[] setores)
        {
            string strSetores = "";

            if (setores.Length == 0)
                return null;
            
            foreach (Setor setor in setores)
            {
                if (strSetores.Length > 0)
                    strSetores += ", ";

                strSetores += setor.Código;
            }

            List<Visita> visitas;

            string where = " v.saida IS NULL OR v.entrada >= date(" +
                "(SELECT MAX(entrada) FROM visita WHERE funcionario = " +
                DbTransformar(funcionário.Código) + "))" +
                " AND setor IN (" + strSetores + ")" +
                " AND entrada >= " + DbTransformar(DadosGlobais.Instância.HoraDataAtual.Date.Subtract(new TimeSpan(3, 0, 0, 0))) +
                " ORDER BY entrada";

            visitas = Mapear<Visita>("SELECT * FROM visita v WHERE " + where);

            CarregarRelacionamentos(visitas, where);

            return visitas;
        }

        public string ExtrairNomes()
        {
            StringBuilder nomes = new StringBuilder();

            foreach (Entidades.Pessoa.Pessoa pessoa in this.pessoas)
            {
                if (nomes.Length > 0)
                    nomes.Append(", ");

                nomes.Append(pessoa.Nome);
            }

            foreach (string nome in this.nomes)
            {
                if (nomes.Length > 0)
                    nomes.Append(", ");

                nomes.Append(nome);
            }

            return nomes.ToString();
        }

        public static string ExtrairNomes(List<Visita> visitas)
        {
            StringBuilder nomes = new StringBuilder();

            foreach (Visita visita in visitas)
            {
                if (nomes.Length > 0)
                    nomes.Append("; ");

                nomes.Append(visita.ExtrairNomes());
            }

            return nomes.ToString();
        }

        protected override void Cadastrar(IDbCommand cmd)
        {
            entrada = DadosGlobais.Instância.HoraDataAtual;
            Console.WriteLine("Entrada da visita: " + entrada.ToString());
            base.Cadastrar(cmd);
        }

        public override bool Equals(object obj)
        {
            if (obj is Visita)
                return entrada == ((Visita)obj).entrada || base.Equals(obj);

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return entrada.GetHashCode();
        }
    }
}
