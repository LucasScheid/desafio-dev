using cnab_entities.dto;
using cnab_services.token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace cnab_api.Controllers
{
    public class LoginController : BaseController
    {
        private readonly ILoginServico _loginServico;

        public LoginController(ILogger<OperacoesController> log, ILoginServico loginServico) : base(log)
        {
            _loginServico = loginServico;
        }

        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Obtém um token de autenticação que será usada na chamada dos métodos da API.", Description = "Obtém o Token de Autenticação para utilizar nos demais métodos da API.")]
        [SwaggerResponse(200, "Token gerado com sucesso.")]
        [SwaggerResponse(400, "Usuário não autorizado.")]
        public async Task<ActionResult<LoginResposta>> Login([FromBody] LoginRequisicao loginRequisicao)
        {
            try
            {
                var result = await _loginServico.Login(loginRequisicao);

                if (result != null && result.Autenticado)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (System.Exception ex)
            {
                return LogErrorMessage(GetControllerName(), GetActionName(), ex.Message);
            }
        }
    }
}
