using cnab_helpers.database;
using System.Text;

namespace cnab_infra.database.data
{
    public class ArquivoSqlCommand : IArquivoSqlCommand
    {
        private readonly ISqlCommandHelper _sqlCommandHelper;

        public ArquivoSqlCommand(ISqlCommandHelper sqlCommandHelper)
        {
            _sqlCommandHelper = sqlCommandHelper;
        }

        public string Adicionar()
        {
            StringBuilder sb = new();

            sb.Append(" INSERT INTO ");
            sb.Append(_sqlCommandHelper.GetCompleteTableName("arquivo", false));
            sb.Append("([tipo]");
            sb.Append(",[data]");
            sb.Append(",[valor]");
            sb.Append(",[cpf]");
            sb.Append(",[cartao]");
            sb.Append(",[hora]");
            sb.Append(",[dono_loja]");
            sb.Append(",[nome_loja]");
            sb.Append(",[data_inclusao])");
            sb.Append(" VALUES ");
            sb.Append("(@tipo");
            sb.Append(",@data");
            sb.Append(",@valor");
            sb.Append(",@cpf");
            sb.Append(",@cartao");
            sb.Append(",@hora");
            sb.Append(",@dono_loja");
            sb.Append(",@nome_loja");
            sb.Append(",@data_inclusao);");
            sb.Append(" SELECT CAST(scope_identity() AS int)");

            return sb.ToString();
        }

        public string ObterTodasLojas()
        {
            StringBuilder sb = new();

            sb.Append("SELECT DISTINCT");
            sb.Append(" RTRIM(LTRIM([nome_loja])) AS nome_loja");
            sb.Append(" FROM ");
            sb.Append(_sqlCommandHelper.GetCompleteTableName("arquivo", true));
            sb.Append(" ORDER BY [nome_loja]");

            return sb.ToString();
        }

        public string ObterDadosLoja()
        {
            StringBuilder sb = new();

            sb.Append("SELECT");
            sb.Append(" [p2].[descricao]");
            sb.Append(",[p2].[sinal]");
            sb.Append(",[p2].[natureza]");
            sb.Append(",[p1].[data]");
            sb.Append(",[p1].[valor]");
            sb.Append(",[p1].[cpf]");
            sb.Append(",[p1].[cartao]");
            sb.Append(",[p1].[hora]");
            sb.Append(",[p1].[dono_loja]");
            sb.Append(",[p1].[nome_loja]");
            sb.Append(" FROM ");
            sb.Append(_sqlCommandHelper.GetCompleteTableName("arquivo", true, "p1"));
            sb.Append(" INNER JOIN ");
            sb.Append(_sqlCommandHelper.GetCompleteTableName("tipo_transacao", true, "p2"));
            sb.Append("ON [p1].[tipo] = [p2].[id]");
            sb.Append(" WHERE [nome_loja] = @nome_loja");
            sb.Append(" ORDER BY [p1].[data], [p1].[hora]");

            return sb.ToString();
        }
    }
}
