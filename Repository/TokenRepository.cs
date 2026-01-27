using IntroductionToAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToAPI.Repository
{
    public class TokenRepository(ApplicationDbContext _dbContext) : ITokenRepository
    {
        public Task<bool> IsRefreshTokenValidAsync(
            string refreshToken, 
            int userId)
        {
            return _dbContext
                     .UserTokens
                     .AnyAsync(
                             x => x.Token == refreshToken
                             && x.UserId == userId
                             && x.ExpiryTime >= DateTime.UtcNow
                             && x.IsRevoked == false
                         );
        }


        public async Task<bool> PersistRefreshTokenAsync(
            RefreshToken token)
        {
            await _dbContext.UserTokens.AddAsync(token);

            return await _dbContext.SaveChangesAsync() > 0;
        }

      
    }
}