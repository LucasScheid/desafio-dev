namespace cnab_infra.database.data
{
    public interface IArquivoSqlCommand
    {
        string Adicionar();

        string ObterTodasLojas();

        string ObterDadosLoja();

    }
}
