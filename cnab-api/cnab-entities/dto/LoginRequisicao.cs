namespace cnab_entities.dto
{
    public class LoginRequisicao
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }

        public LoginRequisicao(string usuario, string senha)
        {
            Usuario = usuario;
            Senha = senha;
        }
    }
}
