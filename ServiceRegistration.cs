using IntroductionToAPI.Data;
using IntroductionToAPI.Repository;
using IntroductionToAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToAPI
{
    public static class ServiceRegistration
    {

        public static IServiceCollection RegisterDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DatabaseConnection")));

            return services;    
        }


        public static IServiceCollection RegisterService(
            this IServiceCollection services)
        {
            services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IUserService, UserService>();
            

            return services;
        }
    }
}
