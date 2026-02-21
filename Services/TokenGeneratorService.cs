using System.Text;
using System.Security.Claims;
using IntroductionToAPI.Data;
using IntroductionToAPI.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using IntroductionToAPI.Models.Response;
using IntroductionToAPI.Models.Request;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using IntroductionToAPI.Repository;

namespace IntroductionToAPI.Services
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly IUserService _userService;
        private readonly JwtModelOption _jwtModelOption;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserRepository _userRepository;

        public TokenGeneratorService(
            IUserService userService,
            IOptions<JwtModelOption> option,
            ITokenRepository tokenRepository,
            IUserRepository userRepository)
        {
            _userService = userService;
            _jwtModelOption = option.Value;
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
        }

        public async Task<TokenResponseDto> GenerateToken(
            string username, string password)
        {
            var userDetails
                 = await _userService.GetEmployeeAsync(username, password);

            if (userDetails == null)
                throw new Exception("Invalid UserName or Password");

            return await GenerateTokenAsync(userDetails);

        }

        private async Task PersistRefreshToken(
            User userDetails,
            TokenResponseDto tokenResponseDto)
        {
            await _tokenRepository.PersistRefreshTokenAsync(
                new RefreshToken()
                {
                    ExpiryTime = DateTime.UtcNow.AddDays(7), // from config
                    IsRevoked = false,
                    GeneratedTime = DateTime.UtcNow,
                    Token = tokenResponseDto.refreshToken,
                    UserId = userDetails.Id
                });
        }

        private async Task<TokenResponseDto> GenerateTokenAsync(
            User userDetails)
        {
            var secretKey = _jwtModelOption.Secret;

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
                        new Claim(ClaimTypes.Role,userDetails.Role),
                        new Claim("user-function", "123")
                    ]),
                     Expires = DateTime.UtcNow.AddMinutes(
                             _jwtModelOption.TokenExpiryInMinutes),
                     SigningCredentials = credentials,
                     Issuer = _jwtModelOption.Issuer,
                     Audience = _jwtModelOption.Audience,
                 };
            var tokenHandler = new JsonWebTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);  // jwt

            var tokenResponse = new TokenResponseDto(token, GenerateRefreshToken(userDetails));

            await PersistRefreshToken(userDetails, tokenResponse);

            return tokenResponse;
        }


        private string GenerateRefreshToken(User user)
        {
            var tokenInformation = string.Concat(user.UserName, user.FirstName);

            return BCrypt.Net.BCrypt.HashPassword(tokenInformation);
        }

        public async Task<TokenResponseDto> GenerateRefreshTokenAsync(
            RefreshTokenRequestDto refreshTokenRequest)
        {
            // Validate access token 
            var claimPrincipal = GetTokenPrincipal(refreshTokenRequest.AccessToken);

            if (claimPrincipal == null) throw new Exception("Access token is invalid");


            // validate refresh token
            int userId =
                Convert.ToInt32(claimPrincipal.Claims.FirstOrDefault(
                        x => x.Type == "UserId")!.Value);

            bool isValidRefreshToken =
                await _tokenRepository.IsRefreshTokenValidAsync(
                        refreshTokenRequest.RefreshToken, userId);

            if (!isValidRefreshToken) throw new Exception("Refresh token is not valid");
            
            var userDetails = await _userRepository.FindUserAsync(userId);

            return await GenerateTokenAsync(userDetails);
        }


        private ClaimsPrincipal GetTokenPrincipal(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _jwtModelOption.Secret;

            var securityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));


            var tokenValidator = TokenValidationParameters(securityKey);

            return tokenHandler.ValidateToken(accessToken, tokenValidator, out _);
        }

        private TokenValidationParameters TokenValidationParameters(
            SymmetricSecurityKey securityKey)
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtModelOption.Issuer,
                ValidAudience = _jwtModelOption.Audience,
                IssuerSigningKey = securityKey,
                ValidateLifetime = false
            };
        }
    }
}
