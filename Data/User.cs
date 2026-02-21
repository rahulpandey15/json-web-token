namespace IntroductionToAPI.Data
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; } = default!;

        public string Password { get; set; } = default!;

        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public string Gender { get; set; } = default!;

        public string Role { get; set; } = default!;
    }
}
