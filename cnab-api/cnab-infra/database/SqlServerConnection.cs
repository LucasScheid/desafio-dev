using cnab_contracts.database;
using System.Data;
using System.Data.SqlClient;

namespace cnab_infra.database
{
    public class SqlServerConnection : IDBConnection, IDisposable
    {
        private readonly SqlConnection _sqlConnection;

        public SqlServerConnection(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public async Task CloseConnection(bool reuseConnection = false)
        {
            if (!reuseConnection && IsOpenConnection())
                await _sqlConnection.CloseAsync();
        }

        public IDbConnection GetConnection(IDbConnection? reuseConnection = null)
        {
            return reuseConnection ?? _sqlConnection;
        }

        private bool IsOpenConnection()
        {
            return _sqlConnection.State == ConnectionState.Open;
        }

        public async Task OpenConnection(bool reuseConnection = false)
        {
            if (!reuseConnection)
            {
                await _sqlConnection.OpenAsync();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

    }
}
