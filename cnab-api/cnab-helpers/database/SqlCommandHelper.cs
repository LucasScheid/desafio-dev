using cnab_helpers.environment;
using System.Text;

namespace cnab_helpers.database
{
    public class SqlCommandHelper : ISqlCommandHelper
    {
        private readonly IEnvironmentHelper _environmentHelper;

        public SqlCommandHelper(IEnvironmentHelper environmentHelper)
        {
            _environmentHelper = environmentHelper;
        }

        public string GetCompleteTableName(string tableName, bool addNoLockInstruction = false, string tableAlias = null)
        {
            StringBuilder sb = new();

            sb.Append('[');
            sb.Append(_environmentHelper.ReadContent("SQLSERVER_DATABASE"));
            sb.Append(']');
            sb.Append('.');
            sb.Append('[');
            sb.Append(_environmentHelper.ReadContent("SQLSERVER_SCHEMA"));
            sb.Append(']');
            sb.Append('.');
            sb.Append('[');
            sb.Append(tableName);
            sb.Append(']');
            sb.Append(addNoLockInstruction ? "(NOLOCK)" : string.Empty);
            sb.Append((!string.IsNullOrEmpty(tableAlias)) ? $" AS [{tableAlias}]" : string.Empty);

            return sb.ToString();
        }
    }
}
