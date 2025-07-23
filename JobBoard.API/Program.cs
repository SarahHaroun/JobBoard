using JobBoard.Domain.Data;
using JobBoard.Repositories.Persistence;
using JobBoard.Services._ِAuthService;
using JobBoard.Services.EmployerService;
using JobBoard.Services.SeekerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JobBoard.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            var configuration = builder.Configuration;

            /*------------------------Add DbContext--------------------------*/
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            /*------------------------Add Identity--------------------------*/
            builder.Services.AddIdentity<UserApplication, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

           
            /*------------------------Add JWT Authentication--------------------------*/
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  //return unAuthorized
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; 
            })
            .AddJwtBearer(options =>         /// Validate the token
            {
                options.RequireHttpsMetadata = true; // Use HTTPS for production
                options.SaveToken = true; // Save the token in the request
                options.TokenValidationParameters = new TokenValidationParameters    
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    ValidAudience = configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])) // Secret key for signing the token
                };
            });
            
            /*------------------------Add Services--------------------------*/
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IEmployerService, EmployerService>();
            builder.Services.AddScoped<ISeekerService, SeekerService>();


            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure Middleware Pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();  
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
