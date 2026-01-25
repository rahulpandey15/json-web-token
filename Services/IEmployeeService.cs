using IntroductionToAPI.Data;

namespace IntroductionToAPI.Services
{
    public interface IEmployeeService
    {
        Task<Employee> GetEmployeeAsync(string userName, string password);
    }
}
