using System.Data;

namespace cnab_contracts.database
{
    public interface IDBConnection
    {
        Task CloseConnection(bool reuseConnection = false);

        Task OpenConnection(bool reuseConnection = false);

        IDbConnection GetConnection(IDbConnection? reuseConnection = null);
    }
}
