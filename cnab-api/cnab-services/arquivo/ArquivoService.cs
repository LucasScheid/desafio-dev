using cnab_entities.dto;
using cnab_entities.enums;
using cnab_entities.map;
using cnab_entities.models;
using cnab_helpers.environment;
using cnab_services.database;
using Microsoft.AspNetCore.Http;

namespace cnab_services.arquivo
{
    public class ArquivoService : IArquivoService<CNAB>
    {
        private readonly int _fileMaxSize;
        private readonly IArquivoMapPosicao _arquivoMapPosicao;
        private readonly ITipoTransacaoDbService _tipoTransacaoDbService;

        public ArquivoService(IEnvironmentHelper enviromentHelper, IArquivoMapPosicao arquivoMapPosicao, ITipoTransacaoDbService tipoTransacaoDbService)
        {
            _fileMaxSize = Convert.ToInt32(enviromentHelper.ReadContent("FILE_UPLOAD_MAX_SIZE", "2097152"));
            _arquivoMapPosicao = arquivoMapPosicao;
            _tipoTransacaoDbService = tipoTransacaoDbService;
        }

        public async Task<ArquivoUploadResult<CNAB>> Upload(List<IFormFile> fileList)
        {
            ArquivoUploadResult<CNAB> arquivoValidationResult = new();

            foreach (var file in fileList)
            {
                CompleteArquivoUpload<CNAB> completeArquivoUpload = new(file.FileName);

                var extensaoArquivo = Path.GetExtension(file.FileName);

                if (string.IsNullOrEmpty(extensaoArquivo))
                {
                    completeArquivoUpload.Erros.Add("Arquivo sem extensão!");
                    completeArquivoUpload.Valido = false;
                    arquivoValidationResult.CompleteArquivosUpload.Add(completeArquivoUpload);
                    continue;
                }

                if (!Enum.TryParse(typeof(ArquivoUploadType), extensaoArquivo.Replace(".", "").ToUpper(), out var tipoArquivo))
                {
                    completeArquivoUpload.Erros.Add($"Extensão {extensaoArquivo} não permitida!");
                    completeArquivoUpload.Valido = false;
                    arquivoValidationResult.CompleteArquivosUpload.Add(completeArquivoUpload);
                    continue;
                }

                var uploadFactory = ArquivoUploadFactory.Create(Enum.Parse<ArquivoUploadType>(extensaoArquivo.Replace(".", "").ToUpper()), file, _fileMaxSize, _arquivoMapPosicao, _tipoTransacaoDbService);

                if (!uploadFactory.Valido())
                {
                    completeArquivoUpload.Erros.Add(uploadFactory.GetMensagemErro());
                    completeArquivoUpload.Valido = false;
                    arquivoValidationResult.CompleteArquivosUpload.Add(completeArquivoUpload);
                    continue;
                }

                if (!await uploadFactory.LinhasValidas())
                {
                    completeArquivoUpload.Erros = uploadFactory.ObterErrosLinhas();
                    completeArquivoUpload.Valido = false;
                }
                else
                {
                    completeArquivoUpload.Resultado = await uploadFactory.MapearColunas();
                    completeArquivoUpload.Valido = true;
                }

                arquivoValidationResult.CompleteArquivosUpload.Add(completeArquivoUpload);
            }

            return arquivoValidationResult;

        }
    }
}
