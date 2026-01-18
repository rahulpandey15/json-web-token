using System.Text;
using IntroductionToAPI.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace IntroductionToAPI.Services
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly IConfiguration configuration;
        private readonly JwtModelOption jwtModelOption;

        public TokenGeneratorService(
            IConfiguration configuration,
            IOptions<JwtModelOption> option)
        {
            this.configuration = configuration;
            jwtModelOption = option.Value;
        }

        public string GenerateToken(
            string username, string password)
        {
            var secretKey = jwtModelOption.Secret;

            var securityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials
                 = new SigningCredentials(securityKey,
                    SecurityAlgorithms.HmacSha256);

            var tokenDescriptor
                 = new SecurityTokenDescriptor
                 {
                     Subject = new System.Security.Claims.ClaimsIdentity([
                            new Claim(ClaimTypes.Name, username),
                            new Claim(ClaimTypes.Role,"Admin"),
                            new Claim("user-function","123")
                         ]),
                     Expires = DateTime.UtcNow.AddMinutes(
                             jwtModelOption.TokenExpiryInMinutes),
                     SigningCredentials = credentials,
                     Issuer = jwtModelOption.Issuer,
                     Audience = jwtModelOption.Audience,
                 };
            var tokenHandler = new JsonWebTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);  // jwt
            return token;


        }
    }
}
