using cnab_entities.dto;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace cnab_services.token
{
    public class GeradorToken : IGeradorToken
    {
        public GeradorToken()
        {
        }

        public LoginResposta GerarToken(LoginRequisicao loginRequisicao)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Convert.FromBase64String(Environment.GetEnvironmentVariable("JWT_SECRET"));

            var claimsIdentity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, loginRequisicao.Usuario)
            });

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = signingCredentials,

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new(true, tokenHandler.WriteToken(token));
        }
    }
}
