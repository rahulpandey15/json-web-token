namespace IntroductionToAPI.Data
{
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Employees { get; set; }

        public DbSet<RefreshToken> UserTokens { get; set; }
    }
}
