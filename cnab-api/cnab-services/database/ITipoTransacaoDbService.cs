using cnab_contracts.database;
using cnab_entities.models;

namespace cnab_services.database
{
    public interface ITipoTransacaoDbService
    {
        Task<IList<TipoTransacao>> ObterTodosRegistros(bool reuseConnection = false, IDBConnection connection = null);
    }
}
