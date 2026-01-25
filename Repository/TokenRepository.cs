using IntroductionToAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IntroductionToAPI.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ApplicationDbContext dbContext;

        public TokenRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<User> ValidateEmployeeAsync(
            string userName, string password)
        {
            return dbContext
                .Employees
                .FirstOrDefaultAsync(x => x.UserName == userName && x.Password == password);
        }
    }
}
