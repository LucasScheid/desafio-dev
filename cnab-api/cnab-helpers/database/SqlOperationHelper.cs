using cnab_contracts.database;
using cnab_entities.enums;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace cnab_helpers.database
{
    public class SqlOperationHelper : ISqlOperationHelper
    {
        private readonly IDBConnection _dBConnection;

        public SqlOperationHelper(IDBConnection dBConnection)
        {
            _dBConnection = dBConnection;
        }

        public List<SqlParameter[]> GetParameters(List<SqlParameter> inputParameters)
        {
            List<SqlParameter> sqlParameters = new();

            foreach (SqlParameter parameter in inputParameters)
            {
                sqlParameters.Add(parameter);
            }

            List<SqlParameter[]> parametersResult = new();
            parametersResult.Add(sqlParameters.ToArray());

            return parametersResult;
        }

        private async Task<ICollection<int>> ExecuteInternal(SqlOperationType sqlOperatiopnType, string sqlQuery, List<SqlParameter[]> sqlParameters = null, bool reuseConnection = false, IDbConnection connection = null)
        {
            try
            {
                await _dBConnection.OpenConnection(reuseConnection);

                ICollection<int> idList = new List<int>();

                using (SqlCommand sqlCommand = new(sqlQuery, (SqlConnection)_dBConnection.GetConnection(connection)))
                {
                    foreach (SqlParameter[] sqlParameter in sqlParameters)
                    {
                        sqlCommand.Parameters.AddRange(sqlParameter);

                        try
                        {
                            switch (sqlOperatiopnType)
                            {
                                case SqlOperationType.INSERT:
                                    idList.Add((Int32)await sqlCommand.ExecuteScalarAsync());
                                    break;

                                case SqlOperationType.UPDATE_OR_DELETE:
                                    idList.Add(await sqlCommand.ExecuteNonQueryAsync());
                                    break;

                                default:
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }

                        sqlCommand.Parameters.Clear();
                    }
                }

                return idList;
            }
            finally
            {
                await _dBConnection.CloseConnection(reuseConnection);
            }
        }


        public async Task<int> Execute(SqlOperationType sqlOperatiopnType, string sqlQuery, SqlParameter[] sqlParameter = null, bool reuseConnection = false, IDbConnection connection = null)
        {
            List<SqlParameter[]> sqlParameters = new();
            sqlParameters.Add(sqlParameter);

            ICollection<int> idList = await ExecuteInternal(sqlOperatiopnType, sqlQuery, sqlParameters, reuseConnection, connection);

            return idList.FirstOrDefault();
        }

        public async Task<ICollection<Dictionary<string, dynamic>>> MapSqlReader(string sqlSelectQuery, List<SqlParameter[]> sqlParameters = null, bool reuseConnection = false, IDbConnection connection = null)
        {
            try
            {
                await _dBConnection.OpenConnection(reuseConnection);

                ICollection<Dictionary<string, dynamic>> map = new List<Dictionary<string, dynamic>>();

                using (SqlCommand sqlCommand = new(sqlSelectQuery, (SqlConnection)_dBConnection.GetConnection(connection)))
                {
                    foreach (SqlParameter[] sqlParameter in sqlParameters)
                    {
                        sqlCommand.Parameters.AddRange(sqlParameter);
                        map = await MapSqlReaderInternal(sqlCommand);
                        sqlCommand.Parameters.Clear();
                    }
                }

                return map;
            }
            finally
            {
                await _dBConnection.CloseConnection(reuseConnection);
            }
        }

        private async Task<ICollection<Dictionary<string, dynamic>>> MapSqlReaderInternal(SqlCommand sqlCommand)
        {
            ICollection<Dictionary<string, dynamic>> map = new List<Dictionary<string, dynamic>>();

            using SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (await sqlDataReader.ReadAsync())
            {
                Dictionary<string, dynamic> mappingSqlReader = new();

                foreach (DbColumn coluna in await sqlDataReader.GetColumnSchemaAsync())
                {
                    dynamic valor = sqlDataReader.GetValue((int)coluna.ColumnOrdinal);
                    mappingSqlReader.Add(coluna.ColumnName, valor.GetType() == typeof(DBNull) ? null : valor);
                }

                map.Add(mappingSqlReader);
            }

            await sqlDataReader.CloseAsync();

            return map;
        }

        public async Task<ICollection<Dictionary<string, dynamic>>> MapSqlReader(string sqlSelectQuery, bool reuseConnection = false, IDbConnection connection = null)
        {
            try
            {
                await _dBConnection.OpenConnection(reuseConnection);

                ICollection<Dictionary<string, dynamic>> map = new List<Dictionary<string, dynamic>>();

                using (SqlCommand sqlCommand = new(sqlSelectQuery, (SqlConnection)_dBConnection.GetConnection(connection)))
                {
                    map = await MapSqlReaderInternal(sqlCommand);
                }

                return map;
            }
            finally
            {
                await _dBConnection.CloseConnection(reuseConnection);
            }
        }
    }
}
