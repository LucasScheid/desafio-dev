using cnab_entities.enums;
using cnab_entities.map;
using cnab_entities.models;
using cnab_services.database;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace cnab_services.arquivo
{
    internal class ArquivoUploadTXT : BaseArquivoUpload, IArquivoUpload<CNAB>
    {
        private readonly IArquivoMapPosicao _arquivoMapPosicao;
        private readonly ITipoTransacaoDbService _tipoTransacaoDbService;

        private readonly PosicaoArquivo _posTipo;
        private readonly PosicaoArquivo _posData;
        private readonly PosicaoArquivo _posValor;
        private readonly PosicaoArquivo _posCPF;
        private readonly PosicaoArquivo _posCartao;
        private readonly PosicaoArquivo _posHora;
        private readonly PosicaoArquivo _posDonoLoja;
        private readonly PosicaoArquivo _posNomeLoja;

        public ArquivoUploadTXT(IFormFile formFile, int fileMaxSize, IArquivoMapPosicao arquivoMapPosicao, ITipoTransacaoDbService tipoTransacaoDbService) : base(formFile, fileMaxSize)
        {
            _arquivoMapPosicao = arquivoMapPosicao;
            _tipoTransacaoDbService = tipoTransacaoDbService;

            _posTipo = _arquivoMapPosicao.ObterPosicaoCampo(NomeCamposCNAB.TIPO);
            _posData = _arquivoMapPosicao.ObterPosicaoCampo(NomeCamposCNAB.DATA);
            _posValor = _arquivoMapPosicao.ObterPosicaoCampo(NomeCamposCNAB.VALOR);
            _posCPF = _arquivoMapPosicao.ObterPosicaoCampo(NomeCamposCNAB.CPF);
            _posCartao = _arquivoMapPosicao.ObterPosicaoCampo(NomeCamposCNAB.CARTAO);
            _posHora = _arquivoMapPosicao.ObterPosicaoCampo(NomeCamposCNAB.HORA);
            _posDonoLoja = _arquivoMapPosicao.ObterPosicaoCampo(NomeCamposCNAB.DONO_LOJA);
            _posNomeLoja = _arquivoMapPosicao.ObterPosicaoCampo(NomeCamposCNAB.NOME_LOJA);
        }

        private void ValidarTipoTransacaoEnviado(IList<TipoTransacao> tiposTransacao, string tipoTransacaoEnviado, int nroLinha)
        {
            int tipo = int.Parse(tipoTransacaoEnviado);

            if (!tiposTransacao.Any(t => t.Id == tipo))
                AddErrosLinha($"{ObterIndicadorLinha(nroLinha)} O valor {NomeCamposCNAB.TIPO} é inválido. Valor enviado: {tipo}");
        }

        public async Task<bool> LinhasValidas()
        {
            int nroLinha = 0;

            var linhas = await ObterLinhas();

            if (linhas.Count == 0)
            {
                AddErrosLinha($"O arquivo não possui linhas de conteúdo.");
                return false;
            }

            IList<TipoTransacao> tiposTransacao = await _tipoTransacaoDbService.ObterTodosRegistros();

            foreach (var linha in linhas)
            {
                nroLinha++;

                try
                {
                    if (CampoBranco(ObterIndicadorLinha(nroLinha), linha, "A LINHA"))
                        continue;

                    ValidarCampoInt(ObterIndicadorLinha(nroLinha), GetConteudo(_posTipo, linha), _posTipo.NomeCampo.ToString());
                    ValidarTipoTransacaoEnviado(tiposTransacao, GetConteudo(_posTipo, linha), nroLinha);
                    ValidarCampoData(ObterIndicadorLinha(nroLinha), GetConteudo(_posData, linha), _posData.NomeCampo.ToString(), "yyyyMMdd");
                    ValidarCampoDecimal(ObterIndicadorLinha(nroLinha), GetConteudo(_posValor, linha), _posValor.NomeCampo.ToString());
                    ValidarCampoBranco(ObterIndicadorLinha(nroLinha), GetConteudo(_posCPF, linha), _posCPF.NomeCampo.ToString());
                    ValidarCampoBranco(ObterIndicadorLinha(nroLinha), GetConteudo(_posCartao, linha), _posCartao.NomeCampo.ToString());
                    ValidarCampoHora(ObterIndicadorLinha(nroLinha), GetConteudo(_posHora, linha), _posHora.NomeCampo.ToString(), "HHmmss");
                    ValidarCampoBranco(ObterIndicadorLinha(nroLinha), GetConteudo(_posDonoLoja, linha), _posDonoLoja.NomeCampo.ToString());
                    ValidarCampoBranco(ObterIndicadorLinha(nroLinha), GetConteudo(_posNomeLoja, linha), _posNomeLoja.NomeCampo.ToString());
                }
                catch (Exception ex)
                {
                    AddErrosLinha($"{ObterIndicadorLinha(nroLinha)} {ex.Message}.");
                }
            }

            return GetErrosLinha().Count == 0;
        }

        public async Task<ICollection<CNAB>> MapearColunas()
        {
            List<CNAB> cnabsUpload = new();

            foreach (var linha in await ObterLinhas())
            {
                cnabsUpload.Add(new CNAB(
                                int.Parse(GetConteudo(_posTipo, linha)),
                                (DateTime.ParseExact(GetConteudo(_posData, linha), "yyyyMMdd", CultureInfo.InvariantCulture)),
                                (decimal.Parse(GetConteudo(_posValor, linha), CultureInfo.InvariantCulture) / 100),
                                GetConteudo(_posCPF, linha),
                                GetConteudo(_posCartao, linha),
                                (DateTime.ParseExact(GetConteudo(_posHora, linha), "HHmmss", CultureInfo.InvariantCulture)).TimeOfDay.ToString(),
                                GetConteudo(_posDonoLoja, linha),
                                GetConteudo(_posNomeLoja, linha)));
            }

            return cnabsUpload;
        }

        public ICollection<string> ObterErrosLinhas()
        {
            return GetErrosLinha();
        }

        public bool Valido()
        {
            return base.ArquivoValido();
        }

        string IArquivoUpload<CNAB>.GetMensagemErro()
        {
            return base.GetMensagemErro();
        }
    }
}
