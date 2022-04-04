using cnab_contracts.database;
using cnab_entities.models;
using cnab_services.database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cnab_unit_tests.back_end
{
    public class TipoTransacaoDbServiceMock : ITipoTransacaoDbService
    {
        public async Task<IList<TipoTransacao>> ObterTodosRegistros(bool reuseConnection = false, IDBConnection? connection = null)
        {
            IList<TipoTransacao> _tiposTransacao = new List<TipoTransacao>
            {
                new TipoTransacao(1, "Débito", "Entrada", "+"),
                new TipoTransacao(2, "Boleto", "Saída", "-"),
                new TipoTransacao(3, "Financiamento", "Saída", "-"),
                new TipoTransacao(4, "Crédito", "Entrada", "+"),
                new TipoTransacao(5, "Recebimento Empréstimo", "Entrada", "+"),
                new TipoTransacao(6, "Vendas", "Entrada", "+"),
                new TipoTransacao(7, "Recebimento TED", "Entrada", "+"),
                new TipoTransacao(8, "Recebimento DOC", "Entrada", "+"),
                new TipoTransacao(9, "Aluguel", "Saída", "-")
            };

            return await Task.FromResult(_tiposTransacao);
        }
    }
}
