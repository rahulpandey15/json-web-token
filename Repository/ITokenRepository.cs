using IntroductionToAPI.Data;

namespace IntroductionToAPI.Repository
{
    public interface ITokenRepository
    {
    
        Task<bool> PersistRefreshTokenAsync(RefreshToken token);

        Task<bool> IsRefreshTokenValidAsync(string refreshToken, int userId);
    }
}
