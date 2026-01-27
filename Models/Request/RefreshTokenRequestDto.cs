namespace IntroductionToAPI.Models.Request;

public class RefreshTokenRequestDto
{
    public string AccessToken { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;
}