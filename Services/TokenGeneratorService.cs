using System.Text;
using System.Security.Claims;
using IntroductionToAPI.Data;
using IntroductionToAPI.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using IntroductionToAPI.Models.Response;
using IntroductionToAPI.Models.Request;
using Microsoft.IdentityModel.JsonWebTokens;

namespace IntroductionToAPI.Services
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly IUserService userService;
        private readonly JwtModelOption jwtModelOption;

        public TokenGeneratorService(
            IUserService userService,
            IOptions<JwtModelOption> option)
        {
            this.userService = userService;
            jwtModelOption = option.Value;
        }

        public async Task<TokenResponseDto> GenerateToken(
            string username, string password)
        {
            var userDetails
                 = await userService.GetEmployeeAsync(username, password);

            if (userDetails == null)
                throw new Exception("Invalid UserName or Password");

            var tokenResponseDto = GenerateToken(userDetails);

            await userService.PersistUserTokenAsync(
                    userDetails.Id, tokenResponseDto.refreshToken);

            return tokenResponseDto;

        }

        private TokenResponseDto GenerateToken(
            User userDetails)
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
                     Subject = new ClaimsIdentity(
                    [
                        new Claim(ClaimTypes.Name, userDetails.UserName),
                        new Claim("UserId",userDetails.Id.ToString()),
                        new Claim(ClaimTypes.Role, "Admin"),
                        new Claim("user-function", "123")
                    ]),
                     Expires = DateTime.UtcNow.AddMinutes(
                             jwtModelOption.TokenExpiryInMinutes),
                     SigningCredentials = credentials,
                     Issuer = jwtModelOption.Issuer,
                     Audience = jwtModelOption.Audience,
                 };
            var tokenHandler = new JsonWebTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);  // jwt

            return new TokenResponseDto(token, GenerateRefreshToken(userDetails));
        }


        private string GenerateRefreshToken(User user)
        {
            var tokenInformation = string.Concat(user.UserName, user.FirstName);
            return BCrypt.Net.BCrypt.HashPassword(tokenInformation);
        }

        public async Task<TokenResponseDto> GenerateRefreshToken(RefreshTokenRequestDto refresh)
        {
            // Validate access token 
            // validate refresh token


            // generate new access token and refres tokem 




            return await Task.FromResult(new TokenResponseDto("", ""));
        }
    }
}
