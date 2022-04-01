using cnab_contracts.database;
using cnab_entities.dto;
using cnab_entities.models;

namespace cnab_services.database
{
    public interface IArquivoDbService
    {
        Task Adicionar(CompleteArquivoUpload<CNAB> arquivoUpload, bool reuseConnection = false, IDBConnection connection = null);

        Task<IList<string>> ObterTodasLojas(bool reuseConnection = false, IDBConnection connection = null);

        Task<OperacaoLojaResult> ObterDadosLoja(string nomeLoja, bool reuseConnection = false, IDBConnection connection = null);

        Task<IList<CNABDatabase>> ObterTodosRegistros(bool reuseConnection = false, IDBConnection connection = null);

    }
}
