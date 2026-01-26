using IntroductionToAPI.Data;
using IntroductionToAPI.Repository;

namespace IntroductionToAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenRepository tokenRepository;

        public UserService(ITokenRepository tokenRepository)
        {
            this.tokenRepository = tokenRepository;
        }

        public async Task<User> GetEmployeeAsync(
            string userName, 
            string password)
        {
            var userDetails = 
                await tokenRepository.ValidateEmployeeAsync(userName, password);

            return userDetails;
        }

        public async Task<bool> PersistUserTokenAsync(
            int userId,
            string refreshToken)
        {

            RefreshToken token
                = new RefreshToken()
                {
                    ExpiryTime = DateTime.UtcNow.AddDays(7), // can be picked from config,
                    GeneratedTime = DateTime.UtcNow,
                    IsRevoked = false,
                    UserId = userId,
                    Token = refreshToken
                };

            return await tokenRepository.PersistRefreshTokenAsync(token);
        }
    }
}
