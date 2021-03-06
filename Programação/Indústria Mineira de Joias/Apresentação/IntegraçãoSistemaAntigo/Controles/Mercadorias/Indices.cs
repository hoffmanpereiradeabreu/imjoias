using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Apresenta��o.Integra��oSistemaAntigo.Controles.Mercadorias
{
	/// <summary>
	/// 
	/// Preenchimento do campo 'coeficiente' de tabela 'mercadoria'
	/// </summary>
	/// 
	/* Esquema para se obter o ind�ce: (seguindo orienta��o do hoffman)
		* 	caso j�ia � peso �nico
			indice = cm_vista
		caso contr�rio

			olho no cadmer a faixa e grupo.
			olho no gramas acho a faixa e grupo da mercadoria.
			preciso do G_VISTA do gramas.
			indice = G_VISTA * peso_especifico_da_joia (mas n�o se sabe o peso_espec�fico_da_j�ia);
*/
	public class Indices
	{
        private const int TABELA_CONSIGNADO = 2;
        private const int TABELA_ATACADO = 3;
        private const int TABELA_ALTO_ATACADO = 4;
        private const int TABELA_VAREJO = 1;
        private const int TABELA_VAREJO_CONSULTA = 5;
        private const int TABELA_REPRESENTANTE = 6;

        private const string COLUNA_CADMER_VAREJO = "VR_3060902";
        private const string COLUNA_CADMER_VAREJO_CONSULTA = "VR_VISTA2";

        private const string MERCADORIA = "mercadoria";
        private const string REFER�NCIA = "referencia";
        private const string TABELA = "tabela";
        private const string GRUPO = "grupo";
        private const string FAIXA = "faixa";

        private double cota��oVarejo;

		private DataTable cadmer, gramas;
		private DataTable tabelaNovaMercadoria, tabelaCoeficiente;
	
		public Indices (DataSet dataSetVelho, DataSet dataSetNovo)
		{
            cadmer = dataSetVelho.Tables["cadmer"];
            gramas = dataSetVelho.Tables["gramas"];
            tabelaNovaMercadoria = dataSetNovo.Tables[MERCADORIA];
            tabelaCoeficiente = dataSetNovo.Tables["tabelamercadoria"];
		}

		public void Transpor(double cota��oVarejo, StringBuilder sa�da)
		{
            int maxComandosPorVez = 200;

            IDbConnection cn = Acesso.MySQL.ConectorMysql.Inst�ncia.CriarConex�o(
                Acesso.MySQL.MySQLUsu�rios.Obter�ltimaStrConex�o());

            cn.Open();

            IDbTransaction t = cn.BeginTransaction();
            
            StringBuilder consulta = new StringBuilder("");

            this.cota��oVarejo = cota��oVarejo;
            
            CriarHash();
            int vezAtual = 0;

            Apresenta��o.Formul�rios.Aguarde aguarde = new Apresenta��o.Formul�rios.Aguarde(
                "Alterando indices do novo banco",
                tabelaNovaMercadoria.Rows.Count, "Transpondo banco de dados", 
                "Aguarde enquanto o banco de dados � sincronizado.");

            aguarde.Abrir();

			foreach(DataRow itemMercadoria in tabelaNovaMercadoria.Rows)
			{
                aguarde.Passo();
    
                TransporMercadoria(itemMercadoria, consulta, sa�da);
                vezAtual++;

                if (vezAtual >= maxComandosPorVez && consulta.Length > 0)
                {
                    vezAtual = 0;
                    IDbCommand cmd = cn.CreateCommand();
                    cmd.Transaction = t;
                    cmd.CommandText = consulta.ToString();
                    consulta.Clear();
                    cmd.ExecuteNonQuery();
                }                
			}

            if (vezAtual > 0 && consulta.Length > 0)
            {
                IDbCommand cmd = cn.CreateCommand();
                cmd.CommandText = consulta.ToString();
                cmd.Transaction = t;
                cmd.ExecuteNonQuery();
                t.Commit();
            }

            aguarde.Close();
		}

        
        // Item do banco de dados antigo
        private Dictionary<string, DataRow> hashRefer�nciaItem, hashTabRefItem;

        private void CriarHash()
        {
            hashRefer�nciaItem = new Dictionary<string, DataRow>(cadmer.Rows.Count, StringComparer.Ordinal);

            foreach (DataRow atual in cadmer.Rows)
                hashRefer�nciaItem.Add(atual["CM_CODMER"].ToString().Trim(), atual);

            hashTabRefItem = new Dictionary<string, DataRow>(tabelaCoeficiente.Rows.Count, StringComparer.Ordinal);

            foreach (DataRow atual in tabelaCoeficiente.Rows)
            {
                hashTabRefItem.Add(atual[TABELA].ToString().Trim() + atual[MERCADORIA].ToString().Trim(), atual);
            }
            
        }

		private void TransporMercadoria(DataRow itemMercadoria, StringBuilder consulta, StringBuilder saida)
		{
			DataRow itemMercadoriaAntiga; 
            double coeficienteAtacado = 0;
            double coeficienteAutoAtacado = 0;
            double valorVarejo = 0;
            double valorVarejoConsulta = 0;
            bool erro = false;
            bool depeso;

            if (!hashRefer�nciaItem.TryGetValue(itemMercadoria[REFER�NCIA].ToString().Trim(), out itemMercadoriaAntiga))
            {
                // saida.AppendLine("Coeficiente: A mercadoria foi apagada do dbf '" + itemMercadoria[REFER�NCIA].ToString() + "'.");
                return;
            }

            depeso = ConferirDePeso(itemMercadoria);
           
            if (depeso)
                CalcularCoeficienteDePeso(itemMercadoria, saida, itemMercadoriaAntiga, ref coeficienteAtacado, ref coeficienteAutoAtacado, ref valorVarejo, ref valorVarejoConsulta, ref erro);
			else 
                CalcularCoeficienteDePe�a(itemMercadoria, saida, itemMercadoriaAntiga, ref coeficienteAtacado, ref coeficienteAutoAtacado, ref valorVarejo, ref valorVarejoConsulta, ref erro);

            DataRow linha;

            linha = TransporAtacado(itemMercadoria, consulta, coeficienteAtacado, erro);
            linha = TranporRepresentante(itemMercadoria, consulta, coeficienteAtacado, erro, linha);
            linha = TransporConsignado(itemMercadoria, consulta, coeficienteAtacado, erro, linha);
            linha = TransporVarejo(itemMercadoria, consulta, valorVarejo, erro, linha);
            linha = TransporVarejoConsulta(itemMercadoria, consulta, valorVarejoConsulta, erro, linha);
            linha = TransporAA(itemMercadoria, consulta, coeficienteAutoAtacado, erro, linha);

            if (erro)
                itemMercadoria["foradelinha"] = true;
            else
            {
                itemMercadoria["depeso"] = depeso;
            }
		}

        private DataRow TransporAA(DataRow itemMercadoria, StringBuilder consulta, double coeficienteAutoAtacado, bool erro, DataRow linha)
        {
            if (hashTabRefItem.TryGetValue(TABELA_ALTO_ATACADO.ToString().Trim() + itemMercadoria[REFER�NCIA].ToString().Trim(), out linha))
            {
                AlterarIndiceExistente(itemMercadoria, TABELA_ALTO_ATACADO, coeficienteAutoAtacado, consulta);
            }
            else if (!erro)
            {
                AdicionarIndiceNaoExistente(itemMercadoria, TABELA_ALTO_ATACADO, coeficienteAutoAtacado, consulta);
            }

            return linha;
        }

        private DataRow TransporVarejoConsulta(DataRow itemMercadoria, StringBuilder consulta, double valorVarejoConsulta, bool erro, DataRow linha)
        {
            if (hashTabRefItem.TryGetValue(TABELA_VAREJO_CONSULTA.ToString().Trim() + itemMercadoria[REFER�NCIA].ToString().Trim(), out linha))
            {
                AlterarIndiceExistente(itemMercadoria, TABELA_VAREJO_CONSULTA, valorVarejoConsulta, consulta);
            }
            else if (!erro)
            {
                AdicionarIndiceNaoExistente(itemMercadoria, TABELA_VAREJO_CONSULTA, valorVarejoConsulta, consulta);
            }
            return linha;
        }

        private DataRow TransporVarejo(DataRow itemMercadoria, StringBuilder consulta, double valorVarejo, bool erro, DataRow linha)
        {
            if (hashTabRefItem.TryGetValue(TABELA_VAREJO.ToString().Trim() + itemMercadoria[REFER�NCIA].ToString().Trim(), out linha))
            {
                AlterarIndiceExistente(itemMercadoria, TABELA_VAREJO, valorVarejo, consulta);
            }
            else if (!erro)
            {
                AdicionarIndiceNaoExistente(itemMercadoria, TABELA_VAREJO, valorVarejo, consulta);
            }
            return linha;
        }

        private DataRow TransporConsignado(DataRow itemMercadoria, StringBuilder consulta, double coeficienteAtacado, bool erro, DataRow linha)
        {
            if (hashTabRefItem.TryGetValue(TABELA_CONSIGNADO.ToString().Trim() + itemMercadoria[REFER�NCIA].ToString().Trim(), out linha))
            {
                AlterarIndiceExistente(itemMercadoria, TABELA_CONSIGNADO, coeficienteAtacado, consulta);
            }
            else if (!erro)
            {
                AdicionarIndiceNaoExistente(itemMercadoria, TABELA_CONSIGNADO, coeficienteAtacado, consulta);
            }
            return linha;
        }

        private DataRow TranporRepresentante(DataRow itemMercadoria, StringBuilder consulta, double coeficienteAtacado, bool erro, DataRow linha)
        {
            if (hashTabRefItem.TryGetValue(TABELA_REPRESENTANTE.ToString().Trim() + itemMercadoria[REFER�NCIA].ToString().Trim(), out linha))
            {
                AlterarIndiceExistente(itemMercadoria, TABELA_REPRESENTANTE, coeficienteAtacado, consulta);
            }
            else if (!erro)
            {
                AdicionarIndiceNaoExistente(itemMercadoria, TABELA_REPRESENTANTE, coeficienteAtacado, consulta);
            }

            return linha;
        }

        private DataRow TransporAtacado(DataRow itemMercadoria, StringBuilder consulta, double coeficienteAtacado, bool erro)
        {
            DataRow linha;
            if (hashTabRefItem.TryGetValue(TABELA_ATACADO.ToString().Trim() + itemMercadoria[REFER�NCIA].ToString().Trim(), out linha))
            {
                AlterarIndiceExistente(itemMercadoria, TABELA_ATACADO, coeficienteAtacado, consulta);
            }
            else if (!erro)
            {
                AdicionarIndiceNaoExistente(itemMercadoria, TABELA_ATACADO, coeficienteAtacado, consulta);
            }
            return linha;
        }

        private static void CalcularCoeficienteDePe�a(DataRow itemMercadoria, StringBuilder saida, DataRow itemMercadoriaAntiga, ref double coeficienteAtacado, ref double coeficienteAutoAtacado, ref double valorVarejo, ref double valorVarejoConsulta, ref bool erro)
        {
            try
            {
                coeficienteAtacado = double.Parse(itemMercadoriaAntiga["CM_VISTA"].ToString());
                coeficienteAutoAtacado = coeficienteAtacado;
                valorVarejo = double.Parse(itemMercadoriaAntiga[COLUNA_CADMER_VAREJO].ToString());
                valorVarejoConsulta = double.Parse(itemMercadoriaAntiga[COLUNA_CADMER_VAREJO_CONSULTA].ToString());
            }
            catch (Exception err)
            {
                saida.AppendLine("Coeficiente: cm_vista � nulo para '" + itemMercadoria[REFER�NCIA].ToString() + "'. coeficienete ser� 99999. " + err.Message);

                coeficienteAtacado
                    = coeficienteAutoAtacado
                    = valorVarejo = valorVarejoConsulta = 99999;

                erro = true;
            }
        }

        private void CalcularCoeficienteDePeso(DataRow itemMercadoria, StringBuilder saida, DataRow itemMercadoriaAntiga, ref double coeficienteAtacado, ref double coeficienteAutoAtacado, ref double valorVarejo, ref double valorVarejoConsulta, ref bool erro)
        {
            try
            {
                coeficienteAtacado =
                    ObterGVistaDeGramas(4, itemMercadoria[FAIXA].ToString(), int.Parse(itemMercadoria[GRUPO].ToString()));

                coeficienteAutoAtacado =
                    ObterGVistaDeGramas(8, itemMercadoria[FAIXA].ToString(), int.Parse(itemMercadoria[GRUPO].ToString()));


                valorVarejo = double.Parse(itemMercadoriaAntiga[COLUNA_CADMER_VAREJO].ToString());
                valorVarejoConsulta = double.Parse(itemMercadoriaAntiga[COLUNA_CADMER_VAREJO_CONSULTA].ToString());
            }
            catch (Exception e)
            {
                saida.AppendLine("Coeficiente: Erro ao cadastrar coeficiente para '" + itemMercadoria[REFER�NCIA].ToString() + "'. valor ser� 99999. " + e.Message);

                coeficienteAtacado
                    = coeficienteAutoAtacado
                    = valorVarejo = valorVarejoConsulta = 99999;

                erro = true;
            }
        }

        private static string DbTransformar(double valor)
        {
            return valor.ToString().Replace(',', '.');
        }

        private static void AlterarIndiceExistente(DataRow itemMercadoria, int tabela, double coeficiente, StringBuilder consulta)
        {

            consulta.Append("update tabelamercadoria set coeficiente = '" + DbTransformar(coeficiente) + "' WHERE "
                + " mercadoria = '" + itemMercadoria[REFER�NCIA].ToString() + "' AND tabela = " + tabela.ToString() + ";");
        }

        private static void AdicionarIndiceNaoExistente(DataRow itemMercadoria, int tabela, double coeficiente, StringBuilder consulta)
        {
            consulta.Append("insert into tabelamercadoria (coeficiente,mercadoria,tabela) VALUES ('" +  DbTransformar(coeficiente) + "',"
                + "'" + itemMercadoria[REFER�NCIA].ToString() + "'," + tabela.ToString() + ");");
        }

		private static bool ConferirDePeso(DataRow mercadoria)
		{
			if (mercadoria["depeso"].ToString() == "1") 
				return true;
			else
				return false;
		}

		/// <summary>
		/// Pe�a � de grama.
		/// Ver no inicio coment�rio no inicio da classe para saber para qu� serve.
		/// </summary>
		/// <param name=FAIXA></param>
		/// <param name=GRUPO></param>
		/// <returns></returns>
		private double ObterGVistaDeGramas(int tabela, string faixa, int grupo)
		{
			foreach(DataRow atual in gramas.Rows)
			{
				if ((int.Parse(atual["G_GRUPO"].ToString()) == grupo)
					&&
					(atual["G_FAIXA"].ToString().Trim().CompareTo(faixa) == 0)
					&&
						(Int32.Parse(atual["G_CODTAB"].ToString()) == tabela))
				{
					return double.Parse(atual["G_VISTA"].ToString());
				}
			}
			throw new Exception("N�o encontrei no gramas.dbf faixa =" + faixa.Trim() + ", grupo ="  + grupo.ToString().Trim() + ", codtab =" + tabela.ToString());
		}
	}
}
