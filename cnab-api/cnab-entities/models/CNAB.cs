using System.Globalization;

namespace cnab_entities.models
{
    public class CNAB
    {
        public int Tipo { get; private set; }

        public DateTime Data { get; private set; }

        public decimal Valor { get; private set; }

        public string CPF { get; set; }

        public string Cartao { get; set; }

        public string? Hora { get; private set; }

        public string DonoLoja { get; set; }

        public string NomeLoja { get; set; }

        public CNAB(string tipo, string data, string valor, string cpf, string cartao, string hora, string donoLoja, string nomeLoja)
        {
            SetTipo(tipo);
            SetData(data);
            SetValor(valor);
            CPF = cpf.Trim();
            Cartao = cartao.Trim();
            SetHora(hora);
            DonoLoja = donoLoja.Trim();
            NomeLoja = nomeLoja.Trim();
        }

        public void SetData(string valor)
        {
            this.Data = DateTime.ParseExact(valor, "yyyyMMdd", CultureInfo.InvariantCulture);
        }

        public void SetTipo(string valor)
        {
            this.Tipo = int.Parse(valor);
        }

        public void SetHora(string valor)
        {
            this.Hora = (DateTime.ParseExact(valor, "HHmmss", CultureInfo.InvariantCulture)).TimeOfDay.ToString();
        }

        public void SetValor(string valor)
        {
            this.Valor = decimal.Parse(valor, CultureInfo.InvariantCulture) / 100;
        }
    }
}
