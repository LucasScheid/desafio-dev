using cnab_services.database;
using Microsoft.AspNetCore.Mvc;

namespace cnab_api.Controllers
{
    public class OperacoesController : BaseController
    {
        private readonly IArquivoDbService _arquivoDbService;

        public OperacoesController(ILogger<OperacoesController> log, IArquivoDbService arquivoDbService) : base(log)
        {
            _arquivoDbService = arquivoDbService;
        }

        [HttpGet]
        [Route("obter-todas-lojas")]
        public async Task<IActionResult> ObterTodasLojas()
        {
            try
            {
                return Ok(await _arquivoDbService.ObterTodasLojas());
            }
            catch (Exception ex)
            {
                return LogErrorMessage(GetControllerName(), GetActionName(), ex.Message);
            }
        }

        [HttpGet]
        [Route("obter-dados-loja")]
        public async Task<IActionResult> ObterDadosLoja(string nomeLoja)
        {
            try
            {
                return Ok(await _arquivoDbService.ObterDadosLoja(nomeLoja));
            }
            catch (Exception ex)
            {
                return LogErrorMessage(GetControllerName(), GetActionName(), ex.Message);
            }
        }

    }
}
