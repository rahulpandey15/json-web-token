namespace IntroductionToAPI.Data
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string Token { get; set; }

        public DateTime GeneratedTime { get; set; }

        public DateTime ExpiryTime { get; set; }

        public int UserId { get; set; }

        public bool IsRevoked { get; set; }
    }
}
