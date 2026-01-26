using IntroductionToAPI.Models.Response;

namespace IntroductionToAPI.Services
{
    public interface ITokenGeneratorService
    {

        Task<TokenResponseDto> GenerateToken(string username, string password);
    }
}
