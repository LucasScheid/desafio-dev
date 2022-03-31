namespace cnab_entities.dto
{
    public class ArquivoUploadResult<T>
    {
        public ICollection<CompleteArquivoUpload<T>> CompleteArquivosUpload { get; set; }
        public ArquivoUploadResult()
        {
            CompleteArquivosUpload = new List<CompleteArquivoUpload<T>>();
        }
    }
}
