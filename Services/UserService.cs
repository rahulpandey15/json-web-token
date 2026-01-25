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
            var userDetails = await tokenRepository.ValidateEmployeeAsync(userName, password);
            return userDetails;
        }
    }
}
