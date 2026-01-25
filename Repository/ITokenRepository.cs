using IntroductionToAPI.Data;

namespace IntroductionToAPI.Repository
{
    public interface ITokenRepository
    {
        Task<Employee> ValidateEmployeeAsync(string userName, string password);
    }
}
