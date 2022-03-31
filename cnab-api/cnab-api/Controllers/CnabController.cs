using cnab_entities.models;
using cnab_services.arquivo;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace cnab_api.Controllers
{   
    public class CnabController : BaseController
    {
        private readonly IArquivoService<CNAB> _arquivoService;

        public CnabController(ILogger<CnabController> log, IArquivoService<CNAB> arquivoService) : base (log)
        {
            _arquivoService = arquivoService;
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("upload-arquivo-cnab")]
        public async Task<IActionResult> UploadArquivoCnab(List<IFormFile> fileList)
        {
            try
            {
                return Ok(await _arquivoService.Upload(fileList));
            }
            catch (Exception ex)
            {
                return LogErrorMessage(GetControllerName(), GetActionName(), ex.Message);
            }
        }
    }
}
