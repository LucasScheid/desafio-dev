using cnab_entities.enums;
using cnab_entities.map;
using cnab_entities.models;
using Microsoft.AspNetCore.Http;

namespace cnab_services.arquivo
{
    internal class ArquivoUploadTXT : BaseArquivoUpload, IArquivoUpload<CNAB>
    {
        private readonly IArquivoMapPosicao _arquivoMapPosicao;

        private readonly PosicaoArquivo _posTipo;
        private readonly PosicaoArquivo _posData;
        private readonly PosicaoArquivo _posValor;
        private readonly PosicaoArquivo _posCPF;
        private readonly PosicaoArquivo _posCartao;
        private readonly PosicaoArquivo _posHora;
        private readonly PosicaoArquivo _posDonoLoja;
        private readonly PosicaoArquivo _posNomeLoja;

        public ArquivoUploadTXT(IFormFile formFile, int fileMaxSize, IArquivoMapPosicao arquivoMapPosicao) : base(formFile, fileMaxSize)
        {
            _arquivoMapPosicao = arquivoMapPosicao;

            _posTipo = _arquivoMapPosicao.ObterPosicaoCampo(NomeCamposCNAB.TIPO);
            _posData = _arquivoMapPosicao.ObterPosicaoCampo(NomeCamposCNAB.DATA);
            _posValor = _arquivoMapPosicao.ObterPosicaoCampo(NomeCamposCNAB.VALOR);
            _posCPF = _arquivoMapPosicao.ObterPosicaoCampo(NomeCamposCNAB.CPF);
            _posCartao = _arquivoMapPosicao.ObterPosicaoCampo(NomeCamposCNAB.CARTAO);
            _posHora = _arquivoMapPosicao.ObterPosicaoCampo(NomeCamposCNAB.HORA);
            _posDonoLoja = _arquivoMapPosicao.ObterPosicaoCampo(NomeCamposCNAB.DONO_LOJA);
            _posNomeLoja = _arquivoMapPosicao.ObterPosicaoCampo(NomeCamposCNAB.NOME_LOJA);
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

            foreach (var linha in linhas)
            {
                nroLinha++;

                try
                {
                    if (CampoBranco(ObterIndicadorLinha(nroLinha), linha, "A LINHA"))
                        continue;

                    ValidarCampoInt(ObterIndicadorLinha(nroLinha), GetConteudo(_posTipo, linha), _posTipo.NomeCampo.ToString());
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
                cnabsUpload.Add(new CNAB(GetConteudo(_posTipo, linha),
                                GetConteudo(_posData, linha),
                                GetConteudo(_posValor, linha),
                                GetConteudo(_posCPF, linha),
                                GetConteudo(_posCartao, linha),
                                GetConteudo(_posHora, linha),
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
