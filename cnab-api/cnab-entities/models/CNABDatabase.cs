namespace cnab_entities.models
{
    public class CNABDatabase : CNAB
    {
        public int Id { get; set; }

        public DateTime DataInclusao { get; set; }

        public CNABDatabase(int tipo, DateTime data, decimal valor, string cpf, string cartao, string hora, string donoLoja, string nomeLoja, int id, DateTime dataInclusao) : base(tipo, data, valor, cpf, cartao, hora, donoLoja, nomeLoja)
        {
            Id = id;
            DataInclusao = dataInclusao;
        }
    }
}
