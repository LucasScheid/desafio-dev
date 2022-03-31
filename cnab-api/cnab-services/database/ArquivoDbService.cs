using cnab_contracts.database;
using cnab_entities.dto;
using cnab_entities.enums;
using cnab_entities.models;
using cnab_helpers.database;
using cnab_infra.database.data;
using System.Data.SqlClient;

namespace cnab_services.database
{
    public class ArquivoDbService : IArquivoDbService
    {
        private readonly IArquivoSqlCommand _arquivoSqlCommand;
        private readonly ISqlOperationHelper _sqlOperationHelper;

        public ArquivoDbService(IArquivoSqlCommand arquivoSqlCommand, ISqlOperationHelper sqlOperationHelper)
        {
            _arquivoSqlCommand = arquivoSqlCommand;
            _sqlOperationHelper = sqlOperationHelper;
        }

        public async Task Adicionar(CompleteArquivoUpload<CNAB> arquivoUpload, bool reuseConnection = false, IDBConnection connection = null)
        {
            foreach (var cnab in arquivoUpload.Resultado)
            {
                await _sqlOperationHelper.Execute(SqlOperationType.INSERT, _arquivoSqlCommand.Adicionar(), ObterSqlParametro(cnab), reuseConnection, null);
            }
        }

        private static SqlParameter[] ObterSqlParametro(CNAB cnab)
        {
            List<SqlParameter> sqlParameters = new()
            {
                new SqlParameter("tipo", cnab.Tipo),
                new SqlParameter("data", cnab.Data),
                new SqlParameter("valor", cnab.Valor),
                new SqlParameter("cpf", cnab.CPF),
                new SqlParameter("cartao", cnab.Cartao),
                new SqlParameter("hora", cnab.Hora),
                new SqlParameter("dono_loja", cnab.DonoLoja),
                new SqlParameter("nome_loja", cnab.NomeLoja),
                new SqlParameter("data_inclusao", DateTime.Now),
            };

            return sqlParameters.ToArray();
        }

        public async Task<IList<string>> ObterTodasLojas(bool reuseConnection = false, IDBConnection connection = null)
        {
            IList<string> lojas = new List<string>();

            foreach (Dictionary<string, dynamic> queryResult in await _sqlOperationHelper.MapSqlReader(_arquivoSqlCommand.ObterTodasLojas(), reuseConnection))
            {
                lojas.Add(queryResult["nome_loja"]);
            }
            return lojas;
        }

        public async Task<OperacaoLojaResult> ObterDadosLoja(string nomeLojaFiltro, bool reuseConnection = false, IDBConnection connection = null)
        {
            IList<OperacaoLojaResumo> operacoesLojaResumo = new List<OperacaoLojaResumo>();

            foreach (Dictionary<string, dynamic> queryResult in await _sqlOperationHelper.MapSqlReader(_arquivoSqlCommand.ObterDadosLoja(), _sqlOperationHelper.GetParameters(new List<SqlParameter> { new SqlParameter("nome_loja", nomeLojaFiltro.Trim()) }), reuseConnection))
            {
                string sinal = queryResult["sinal"];
                decimal valor = queryResult["valor"];

                operacoesLojaResumo.Add(new OperacaoLojaResumo((queryResult["descricao"]),
                                                                (queryResult["natureza"]),
                                                                (queryResult["data"]),
                                                                (sinal.Equals("-") ? (valor * -1) : valor),
                                                                (queryResult["cpf"]),
                                                                (queryResult["cartao"]),
                                                                (queryResult["hora"]),
                                                                (queryResult["dono_loja"])));
            }

            if (operacoesLojaResumo.Count == 0)
                nomeLojaFiltro = string.Empty;

            return new OperacaoLojaResult(operacoesLojaResumo, operacoesLojaResumo.Sum(s => s.Valor), nomeLojaFiltro);
        }
    }
}
