using cnab_contracts.database;
using cnab_entities.models;
using cnab_helpers.database;
using cnab_infra.database.data;

namespace cnab_services.database
{
    public class TipoTransacaoDbService : ITipoTransacaoDbService
    {
        private readonly ITipoTransacaoSqlCommand _tipoTransacaoSqlCommand;
        private readonly ISqlOperationHelper _sqlOperationHelper;

        public TipoTransacaoDbService(ITipoTransacaoSqlCommand tipoTransacaoSqlCommand, ISqlOperationHelper sqlOperationHelper)
        {
            _tipoTransacaoSqlCommand = tipoTransacaoSqlCommand;
            _sqlOperationHelper = sqlOperationHelper;
        }

        public async Task<IList<TipoTransacao>> ObterTodosRegistros(bool reuseConnection = false, IDBConnection connection = null)
        {
            IList<TipoTransacao> tiposTransacao = new List<TipoTransacao>();

            foreach (Dictionary<string, dynamic> queryResult in await _sqlOperationHelper.MapSqlReader(_tipoTransacaoSqlCommand.ObterTodosRegistros(), reuseConnection))
            {
                tiposTransacao.Add(new TipoTransacao(queryResult["id"], queryResult["descricao"], queryResult["natureza"], queryResult["sinal"]));
            }

            return tiposTransacao;
        }
    }
}
