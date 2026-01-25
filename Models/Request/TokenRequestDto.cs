namespace IntroductionToAPI.Models.Request
{
    public class TokenRequestDto
    {
        public string UserName { get; set; } = default!;

        public string Password { get; set; } = default!;
    }
}
