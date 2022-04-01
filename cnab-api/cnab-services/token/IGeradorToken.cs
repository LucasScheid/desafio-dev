using cnab_entities.dto;

namespace cnab_services.token
{
    public interface IGeradorToken
    {
        LoginResposta GerarToken(LoginRequisicao loginRequisicao);
    }
}
