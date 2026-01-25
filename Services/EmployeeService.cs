using IntroductionToAPI.Data;
using IntroductionToAPI.Repository;

namespace IntroductionToAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ITokenRepository tokenRepository;

        public EmployeeService(ITokenRepository tokenRepository)
        {
            this.tokenRepository = tokenRepository;
        }

        public async Task<Employee> GetEmployeeAsync(
            string userName, 
            string password)
        {
            var employeeDetails = await tokenRepository.ValidateEmployeeAsync(userName, password);
            return employeeDetails;
        }
    }
}
