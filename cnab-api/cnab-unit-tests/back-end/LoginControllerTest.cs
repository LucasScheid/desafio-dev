using cnab_entities.dto;
using cnab_services.token;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xunit;

namespace cnab_unit_tests.back_end
{
    public class LoginControllerTest
    {
        private readonly IGeradorToken _geradorToken;
        private readonly ILoginServico _loginServico;

        public LoginControllerTest()
        {
            _geradorToken = new GeradorToken();
            _loginServico = new LoginServico(_geradorToken);

            AjustarVariaveisAmbiente();
        }

        [Fact]
        public async Task LoginSucess()
        {
            LoginRequisicao loginRequisicao = new("api-cnab-user", "VN403HYdpzbDtfphmBeU");
            LoginResposta loginResposta = await _loginServico.Login(loginRequisicao);

            Assert.True(loginResposta.Autenticado);
        }

        [Fact]
        public async Task LoginFail()
        {
            LoginRequisicao loginRequisicao = new("api-cnab-userxxxx", "xxxxxxVN403HYdpzbDtfphmBeUxxxxxxx");
            LoginResposta loginResposta = await _loginServico.Login(loginRequisicao);

            Assert.False(loginResposta.Autenticado);
        }

        private static void AjustarVariaveisAmbiente()
        {
            Environment.SetEnvironmentVariable("JWT_SECRET", Convert.ToBase64String(new HMACSHA256().Key), EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("API_INTERNAL_USER", "api-cnab-user");
            Environment.SetEnvironmentVariable("API_INTERNAL_PASSWORD", "VN403HYdpzbDtfphmBeU");
            Environment.SetEnvironmentVariable("JWT_ISSUER", "api-cnab-issuer");
            Environment.SetEnvironmentVariable("JWT_AUDIENCE", "api-cnab-audience");
        }
    }
}
