namespace cnab_entities.dto
{
    public class OperacaoLojaResult
    {
        public OperacaoLojaResult(IList<OperacaoLojaResumo> operacoes, decimal saldo, string nomeLoja)
        {
            Operacoes = operacoes;
            Saldo = saldo;
            NomeLoja = nomeLoja;
        }

        public IList<OperacaoLojaResumo> Operacoes { get; set; }

        public decimal Saldo { get; set; }

        public string NomeLoja { get; set; }

    }
}
