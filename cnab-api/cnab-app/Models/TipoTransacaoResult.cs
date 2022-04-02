using cnab_entities.models;

namespace cnap_app.Models
{
    public class TipoTransacaoResult
    {
        public ICollection<TipoTransacao> Registros { get; set; }

        public TipoTransacaoResult(ICollection<TipoTransacao> registros)
        {        
            Registros = registros;
        }
    }
}
