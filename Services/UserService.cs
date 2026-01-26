using IntroductionToAPI.Data;
using IntroductionToAPI.Repository;

namespace IntroductionToAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetEmployeeAsync(
            string userName,
            string password)
        {
            var userDetails =
                await _userRepository.ValidateEmployeeAsync(userName, password);

            return userDetails;
        }


    }
}
