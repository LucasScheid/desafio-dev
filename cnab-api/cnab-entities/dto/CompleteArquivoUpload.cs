namespace cnab_entities.dto
{
    public class CompleteArquivoUpload<T>
    {
        public string Nome { get; set; }

        public bool Valido { get; set; }

        public ICollection<string> Erros { get; set; }

        public ICollection<T> Resultado { get; set; }

        public CompleteArquivoUpload(string fileName)
        {
            Erros = new List<string>();
            Resultado = new List<T>();
            Nome = fileName;

        }
    }
}
