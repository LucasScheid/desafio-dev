using cnab_entities.enums;
using System.Data;
using System.Data.SqlClient;

namespace cnab_helpers.database
{
    public interface ISqlOperationHelper
    {
        Task<ICollection<Dictionary<string, dynamic>>> MapSqlReader(string sqlSelectQuery, List<SqlParameter[]> sqlParameters = null, bool reuseConnection = false, IDbConnection connection = null);

        Task<ICollection<Dictionary<string, dynamic>>> MapSqlReader(string sqlSelectQuery, bool reuseConnection = false, IDbConnection connection = null);

        Task<int> Execute(SqlOperationType sqlOperatiopnType, string sqlQuery, SqlParameter[] sqlParameter = null, bool reuseConnection = false, IDbConnection connection = null);

        List<SqlParameter[]> GetParameters(List<SqlParameter> inputParameters);
    }
}
