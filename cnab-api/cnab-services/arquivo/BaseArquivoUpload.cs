using cnab_entities.map;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Text;

namespace cnab_services.arquivo
{
    internal abstract class BaseArquivoUpload
    {
        private readonly IFormFile _file;
        private string MensagemErro = String.Empty;
        private readonly bool _valido;
        private readonly ICollection<string> _linhas = new List<string>();
        private readonly ICollection<string> _errosLinha = new List<string>();

        protected BaseArquivoUpload(IFormFile file, int fileMaxSize)
        {
            _file = file;
            _valido = ValidarArquivo(fileMaxSize);
        }

        protected void AddErrosLinha(string erro)
        {
            _errosLinha.Add(erro);
        }

        protected ICollection<string> GetErrosLinha()
        {
            return _errosLinha;
        }

        protected static string ObterIndicadorLinha(int numeroLinha)
        {
            return $"Erro linha {numeroLinha} :";
        }

        protected void ValidarCampoDecimal(string indicadorLinha, string valor, string textoCampo)
        {
            if (string.IsNullOrEmpty(valor) || string.IsNullOrWhiteSpace(valor))
                _errosLinha.Add($"{indicadorLinha} {textoCampo} está em branco.");
            else
            {
                bool conversaoOK = decimal.TryParse(valor, out decimal valorConvertido);

                if (!conversaoOK)
                    _errosLinha.Add($"{indicadorLinha} O campo {textoCampo} é inválido. Valor enviado: {valor}");
                else
                {
                    if (valorConvertido <= 0)
                        _errosLinha.Add($"{indicadorLinha} O campo {textoCampo} deve ser maior que zero. Valor enviado: {valor}");
                }
            }
        }

        protected void ValidarCampoData(string indicadorLinha, string valor, string textoCampo, string formatoData)
        {
            if (string.IsNullOrEmpty(valor) || string.IsNullOrWhiteSpace(valor))
            {
                _errosLinha.Add($"{indicadorLinha} {textoCampo} está em branco.");
            }
            else
            {
                if (!DateTime.TryParseExact(valor, formatoData, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    _errosLinha.Add($"{indicadorLinha} O campo {textoCampo} é inválido. Valor enviado: {valor}");
                }
            }
        }

        protected void ValidarCampoHora(string indicadorLinha, string valor, string textoCampo, string formatoHora)
        {
            if (string.IsNullOrEmpty(valor) || string.IsNullOrWhiteSpace(valor))
            {
                _errosLinha.Add($"{indicadorLinha} {textoCampo} está em branco.");
            }
            else
            {
                if (!DateTime.TryParseExact(valor, formatoHora, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    _errosLinha.Add($"{indicadorLinha} O campo {textoCampo} é inválido. Valor enviado: {valor}");
                }
            }
        }

        protected void ValidarCampoInt(string indicadorLinha, string valor, string textoCampo, bool validarMaiorZero = false)
        {
            if (string.IsNullOrEmpty(valor) || string.IsNullOrWhiteSpace(valor))
            {
                _errosLinha.Add($"{indicadorLinha} {textoCampo} está em branco.");
            }
            else
            {
                if (valor.Contains('.') || valor.Contains(','))
                {
                    _errosLinha.Add($"{indicadorLinha} O campo {textoCampo} NÃO pode conter ponto(.) e nem vírgula(,) pois é inteiro. Valor enviado: {valor}");
                    return;
                }

                if (!int.TryParse(valor, out int campoInt))
                {
                    _errosLinha.Add($"{indicadorLinha} O campo {textoCampo} é inválido. Valor enviado: {valor}");
                }
                else
                {
                    if (validarMaiorZero && campoInt <= 0)
                    {
                        _errosLinha.Add($"{indicadorLinha} O campo {textoCampo} precisa ser maior que zero. Valor enviado: {valor}");
                    }
                }
            }
        }

        protected void ValidarCampoBranco(string indicadorLinha, string valor, string textoCampo)
        {
            if (string.IsNullOrWhiteSpace(valor) || string.IsNullOrEmpty(valor))
            {
                _errosLinha.Add($"{indicadorLinha} {textoCampo} está em branco.");
            }
        }

        protected bool CampoBranco(string indicadorLinha, string valor, string textoCampo)
        {
            if (string.IsNullOrEmpty(valor) || string.IsNullOrWhiteSpace(valor))
            {
                _errosLinha.Add($"{indicadorLinha} {textoCampo} está em branco.");
                return true;
            }
            else
            {
                return false;
            }
        }

        protected bool ArquivoValido()
        {
            return _valido;
        }

        protected static string GetConteudo(PosicaoArquivo posicaoCampoArquivo, string linha)
        {
            string conteudo = string.Empty;
            int tamanhoLinha = linha.Length;

            if (posicaoCampoArquivo.UltimoCampo)
            {
                if (posicaoCampoArquivo.Inicio <= tamanhoLinha)
                    conteudo = linha[posicaoCampoArquivo.Inicio..];
            }
            else
            {
                if (posicaoCampoArquivo.Inicio <= tamanhoLinha && posicaoCampoArquivo.Fim <= tamanhoLinha)
                    conteudo = linha[posicaoCampoArquivo.Inicio..posicaoCampoArquivo.Fim];
            }

            return conteudo;
        }

        protected string GetMensagemErro()
        {
            return MensagemErro;
        }

        protected async Task<ICollection<string>> ObterLinhas()
        {
            if (_linhas.Count == 0)
            {
                using var reader = new StreamReader(_file.OpenReadStream(), Encoding.UTF8);

                while (reader.Peek() >= 0)
                {
                    var linha = await reader.ReadLineAsync();
                    _linhas.Add(string.IsNullOrEmpty(linha) ? string.Empty : linha);
                }
            }

            return _linhas;
        }

        private bool ValidarArquivo(int fileMaxSize)
        {
            if (fileMaxSize == 0)
                fileMaxSize = 2097152;

            if (_file.Length == 0)
            {
                MensagemErro = "Arquivo vazio!";
                return false;
            }

            if (_file.Length > fileMaxSize)
            {
                MensagemErro = "Arquivo supera 2 mega!";
                return false;
            }

            return true;
        }
    }
}
