using IntroductionToAPI.Data;

namespace IntroductionToAPI.Services
{
    public interface IUserService
    {
        Task<User> GetEmployeeAsync(string userName, string password);
    }
}
