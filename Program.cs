using System.Text;
using IntroductionToAPI.Data;
using IntroductionToAPI.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();
        builder.Services.AddOpenApi();

        builder.Services.RegisterService();
        builder.Services.RegisterDatabase(builder.Configuration);

        builder.Services.AddOptions<JwtModelOption>()
            .Bind(builder.Configuration.GetSection("Jwt"));

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero
                };
            });


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        // Apply database migrations on startup
        using (var scope = app.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();

            try
            {
                // Apply pending migrations
                db.Database.Migrate();
            }
            catch (Exception ex)
            {
                var logger = scopedServices.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while migrating the database.");
                throw;
            }

            // Seed database with five employees
            if (!db.Users.Any())
            {
                db.Users.AddRange(new[]
                {
                    new User { 
                        UserName = "john@gmail.com", 
                        Password = "password@123", 
                        FirstName = "John", LastName = "Doe", Gender = "Male" },
                    new User
                    {
                        UserName = "jane@gmail.com",
                        Password = "password@123",
                        FirstName = "Jane",
                        LastName = "Smith",
                        Gender = "Female"
                    },
                    new User 
                    { 
                        UserName = "bob@gmail.com", 
                        Password = "password@123", 
                        FirstName = "Bob", 
                        LastName = "Johnson", 
                        Gender = "Male" 
                    },
                    new User 
                    { 
                        UserName = "alice@gmail.com", 
                        Password = "password@123", 
                        FirstName = "Alice", 
                        LastName = "Williams", 
                        Gender = "Female" 
                    },
                    new User 
                    { 
                        UserName = "charlie@gmail.com", 
                        Password = "password@123", 
                        FirstName = "Charlie", 
                        LastName = "Brown", 
                        Gender = "Male" 
                    }
                });

                db.SaveChanges();
            }
        }

        app.Run();
    }
}
