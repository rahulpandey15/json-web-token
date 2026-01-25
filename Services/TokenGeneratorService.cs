using System.Text;
using IntroductionToAPI.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using IntroductionToAPI.Models.Response;
using IntroductionToAPI.Data;

namespace IntroductionToAPI.Services
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly IEmployeeService employeeService;
        private readonly JwtModelOption jwtModelOption;

        public TokenGeneratorService(
            IEmployeeService employeeService,
            IOptions<JwtModelOption> option)
        {
            this.employeeService = employeeService;
            jwtModelOption = option.Value;
        }

        public async Task<TokenResponseDto> GenerateToken(
            string username, string password)
        {
            var employeeDetails
                 = await employeeService.GetEmployeeAsync(username, password);

            if (employeeDetails == null)
                return null;
            return GenerateToken(employeeDetails);

        }

        private TokenResponseDto GenerateToken(
            Employee employeeDetails)
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
                     Subject = new System.Security.Claims.ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, employeeDetails.UserName),
                        new Claim(ClaimTypes.Role, "Admin"),
                        new Claim("user-function", "123")
                    }),
                     Expires = DateTime.UtcNow.AddMinutes(
                             jwtModelOption.TokenExpiryInMinutes),
                     SigningCredentials = credentials,
                     Issuer = jwtModelOption.Issuer,
                     Audience = jwtModelOption.Audience,
                 };
            var tokenHandler = new JsonWebTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);  // jwt

            return new TokenResponseDto(token, "");
        }
    }
}
