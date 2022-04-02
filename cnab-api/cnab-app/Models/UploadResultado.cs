using cnab_entities.models;

namespace cnap_app.Models
{
    public class UploadResultado
    {
        public string Mensagem { get; set; }

        public ICollection<CNAB> Registros { get; set; }

        public UploadResultado(string mensagem, ICollection<CNAB> registros)
        {
            Mensagem = mensagem;
            Registros = registros;
        }
    }
}
