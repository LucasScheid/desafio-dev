namespace cnab_entities.dto
{
    public class OperacaoLojaResumo
    {
        public OperacaoLojaResumo(string operacao, string natureza, DateTime data, decimal valor, string cpf, string cartao, string hora, string donoLoja)
        {
            Operacao = operacao;
            Natureza = natureza;
            Data = data;
            Valor = valor;
            CPF = cpf;
            Cartao = cartao;
            Hora = hora;
            DonoLoja = donoLoja;
        }

        public string DonoLoja { get; set; }

        public string Operacao { get; set; }

        public string Natureza { get; set; }

        public DateTime Data { get; set; }

        public string Hora { get; set; }

        public decimal Valor { get; set; }

        public string CPF { get; set; }

        public string Cartao { get; set; }
    }
}
