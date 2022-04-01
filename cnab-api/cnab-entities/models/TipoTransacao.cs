namespace cnab_entities.models
{
    public class TipoTransacao
    {
        public int Id { get; set; }
        public string Descricao { get; set; }

        public string Natureza { get; set; }

        public string Sinal { get; set; }
        public TipoTransacao(int id, string descricao, string natureza, string sinal)
        {
            Id = id;
            Descricao = descricao;
            Natureza = natureza;
            Sinal = sinal;
        }
    }
}
