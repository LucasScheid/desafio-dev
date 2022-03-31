using cnab_entities.dto;
using cnab_entities.models;
using cnab_services.arquivo;
using cnab_services.database;
using Microsoft.AspNetCore.Mvc;

namespace cnab_api.Controllers
{
    public class CnabController : BaseController
    {
        private readonly IArquivoService<CNAB> _arquivoService;
        private readonly IArquivoDbService _arquivoDbService;

        public CnabController(ILogger<CnabController> log, IArquivoService<CNAB> arquivoService, IArquivoDbService arquivoDbService) : base(log)
        {
            _arquivoService = arquivoService;
            _arquivoDbService = arquivoDbService;
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("upload-arquivo-cnab")]
        public async Task<IActionResult> UploadArquivoCnab(List<IFormFile> fileList)
        {
            try
            {
                ArquivoUploadResult<CNAB> resultadoUpload = await _arquivoService.Upload(fileList);

                if (resultadoUpload.CompleteArquivosUpload.Count > 0 && resultadoUpload.CompleteArquivosUpload.FirstOrDefault() != null)
                {
                    CompleteArquivoUpload<CNAB>? cnab = resultadoUpload.CompleteArquivosUpload.FirstOrDefault();

                    if (cnab != null && cnab.Valido)
                        await _arquivoDbService.Adicionar(cnab);
                }

                return Ok(resultadoUpload);
            }
            catch (Exception ex)
            {
                return LogErrorMessage(GetControllerName(), GetActionName(), ex.Message);
            }
        }

    }
}
