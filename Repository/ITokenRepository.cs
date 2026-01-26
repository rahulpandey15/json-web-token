using IntroductionToAPI.Data;

namespace IntroductionToAPI.Repository
{
    public interface ITokenRepository
    {
        Task<User> ValidateEmployeeAsync(string userName, string password);

        Task<bool> PersistRefreshTokenAsync(RefreshToken token);
    }
}
