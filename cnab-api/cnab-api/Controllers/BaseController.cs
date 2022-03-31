using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace cnab_api.Controllers
{
    [EnableCors]
    [Route("[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly ILogger _log;
        protected BaseController(ILogger log)
        {
            _log = log;
        }

        protected string GetControllerName()
        {
            return ControllerContext.ActionDescriptor.ControllerName;
        }

        protected string GetActionName()
        {
            return ControllerContext.ActionDescriptor.ActionName;
        }

        protected BadRequestObjectResult LogErrorMessage(string className, string methodName, string errorMessage)
        {
            string fullMessage = $"Erro em => [{className} Controller] Método => [{methodName}] Mensagem de Erro => [{errorMessage}]";
            _log.LogError(fullMessage);
            return BadRequest(fullMessage);
        }

        protected BadRequestObjectResult LogErrorMessage(string errorMessage)
        {
            _log.LogError(errorMessage);
            return BadRequest(errorMessage);
        }

        protected string GetModelStateErrors(ModelStateDictionary modelError)
        {
            return string.Join(";", modelError.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
        }
    }
}
