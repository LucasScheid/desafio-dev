using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace cnab_api.Controllers
{   
    public class CnabController : BaseController
    {
        public CnabController(ILogger<CnabController> log): base (log)
        {
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("upload-arquivo-cnab")]
        public async Task<IActionResult> UploadArquivoCnab(List<IFormFile> fileList)
        {
            try
            {
                return await Task.FromResult(Ok());
            }
            catch (Exception ex)
            {
                return LogErrorMessage(GetControllerName(), GetActionName(), ex.Message);
            }
        }
    }
}
