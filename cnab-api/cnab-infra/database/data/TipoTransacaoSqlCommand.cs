using cnab_helpers.database;
using System.Text;

namespace cnab_infra.database.data
{
    public class TipoTransacaoSqlCommand : ITipoTransacaoSqlCommand
    {

        private readonly ISqlCommandHelper _sqlCommandHelper;

        public TipoTransacaoSqlCommand(ISqlCommandHelper sqlCommandHelper)
        {
            _sqlCommandHelper = sqlCommandHelper;
        }

        public string ObterTodosRegistros()
        {
            StringBuilder sb = new();

            sb.Append("SELECT");
            sb.Append(" [id]");
            sb.Append(",[descricao]");
            sb.Append(",[natureza]");
            sb.Append(",[sinal]");
            sb.Append(" FROM ");
            sb.Append(_sqlCommandHelper.GetCompleteTableName("tipo_transacao", true));
            sb.Append(" ORDER BY [id]");

            return sb.ToString();
        }
    }
}
