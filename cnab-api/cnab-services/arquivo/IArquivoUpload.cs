namespace cnab_services.arquivo
{
    internal interface IArquivoUpload<T>
    {
        Task<ICollection<T>> MapearColunas();

        Task<bool> LinhasValidas();

        ICollection<string> ObterErrosLinhas();

        bool Valido();

        string GetMensagemErro();
    }
}
