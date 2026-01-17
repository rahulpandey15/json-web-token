using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace IntroductionToAPI.Services
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly IConfiguration configuration;

        public TokenGeneratorService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateToken(string username, string password)
        {
            var secretKey = configuration["Jwt:Secret"]!;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials
                 = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor
                 = new SecurityTokenDescriptor
                 {
                     Subject = new System.Security.Claims.ClaimsIdentity([
                            new Claim(ClaimTypes.Name, username),
                            new Claim(ClaimTypes.Role,"Admin"),
                            new Claim("user-function","123")
                         ]),
                     Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(configuration["Jwt:TokenExpiryInMinutes"])),
                     SigningCredentials = credentials,
                     Issuer = configuration["Jwt:Issuer"],
                     Audience = configuration["Jwt:Audience"]

                 };

            var tokenHandler = new JsonWebTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);  // jwt

            return token;


        }
    }
}
