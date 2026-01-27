using IntroductionToAPI.Data;

namespace IntroductionToAPI.Repository
{
    public interface IUserRepository
    {
        Task<User> ValidateEmployeeAsync(string userName, string password);

        Task<User> FindUserAsync(int userId);
    }
}
