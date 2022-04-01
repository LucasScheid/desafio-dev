using cnab_entities.dto;

namespace cnab_services.token
{
    public class LoginServico : ILoginServico
    {
        private readonly IGeradorToken _geradorToken;

        public LoginServico(IGeradorToken geradorToken)
        {
            _geradorToken = geradorToken;
        }

        public async Task<LoginResposta> Login(LoginRequisicao loginRequisicao)
        {
            try
            {
                LoginResposta loginResposta = new(false, String.Empty);

                string user = Environment.GetEnvironmentVariable("API_INTERNAL_USER");
                string senha = Environment.GetEnvironmentVariable("API_INTERNAL_PASSWORD");

                if (user != null && user.Equals(loginRequisicao.Usuario) && senha != null && senha.Equals(loginRequisicao.Senha))
                    loginResposta = _geradorToken.GerarToken(loginRequisicao);

                return await Task.FromResult(loginResposta);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
