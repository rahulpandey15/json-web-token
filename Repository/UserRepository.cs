using IntroductionToAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToAPI.Repository
{
    public class UserRepository(ApplicationDbContext _dbContext) : IUserRepository
    {
        public async Task<User> FindUserAsync(int userId) => await _dbContext.Users.FindAsync(userId);

        public async Task<User> ValidateEmployeeAsync(
          string userName, string password)
        {
            var userDetails
                = await _dbContext.Users
                    .FirstOrDefaultAsync(
                        x => x.UserName == userName && x.Password == password);

            return userDetails;
        }
    }
}
