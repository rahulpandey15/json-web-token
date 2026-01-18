namespace IntroductionToAPI.Services
{
    public interface ITokenGeneratorService
    {
        string GenerateToken(string username, string password);
    }
}
