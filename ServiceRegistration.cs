using IntroductionToAPI.Data;
using IntroductionToAPI.Repository;
using IntroductionToAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToAPI
{
    public static class ServiceRegistration
    {

        extension(IServiceCollection services)
        {
            public IServiceCollection RegisterDatabase(IConfiguration configuration)
            {
                services.AddDbContext<ApplicationDbContext>(
                    options => options.UseSqlServer(configuration.GetConnectionString("DatabaseConnection")));

                return services;    
            }
            
            public IServiceCollection RegisterService()
            {
                services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
                services.AddScoped<ITokenRepository, TokenRepository>();
                services.AddScoped<IUserService, UserService>();
            

                return services;
            }
        }
    }
}
