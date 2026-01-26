using IntroductionToAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToAPI.Repository
{
    public class TokenRepository(ApplicationDbContext dbContext) : ITokenRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<bool> PersistRefreshTokenAsync(
            RefreshToken token)
        {
            await _dbContext.UserTokens.AddAsync(token);
            return await _dbContext.SaveChangesAsync() > 0;
        }

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