namespace cnab_entities.dto
{
    public class LoginResposta
    {
        public bool Autenticado { get; set; }
        public string Token { get; set; }

        public LoginResposta(bool autenticado, string token)
        {
            Autenticado = autenticado;
            Token = token;
        }
    }
}
