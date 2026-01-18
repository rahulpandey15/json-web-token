namespace IntroductionToAPI.Options
{
    public class JwtModelOption
    {
        public string Secret { get; set; } = default!;
        public string Issuer {  get; set; } = default!;
        public string Audience { get; set;  } = default!;
        public int TokenExpiryInMinutes {  get; set; }
    }
}
