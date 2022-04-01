namespace cnab_entities.models
{
    public class CNAB
    {
        public int Tipo { get; set; }

        public DateTime Data { get; set; }

        public decimal Valor { get; set; }

        public string CPF { get; set; }

        public string Cartao { get; set; }

        public string? Hora { get; set; }

        public string DonoLoja { get; set; }

        public string NomeLoja { get; set; }

        public CNAB(int tipo, DateTime data, decimal valor, string cpf, string cartao, string hora, string donoLoja, string nomeLoja)
        {
            Tipo = tipo;
            Data = data;
            Valor = valor;
            CPF = cpf.Trim();
            Cartao = cartao.Trim();
            Hora = hora;
            DonoLoja = donoLoja.Trim();
            NomeLoja = nomeLoja.Trim();
        }
    }
}
