using System.Data;
using System.Text;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using ToTask.Data;
using ToTask.DTOs;
using ToTask.Services;
using ToTask.Utils;

namespace ToTask
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUserValidator>());;

            // Configure Dapper and database connection
            string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? Configuration["ConnectionStrings:DefaultConnection"];
            
            try
            {
                IDbConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                connection.Close();
                services.AddScoped<IDbConnection>(provider => connection);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to establish a database connection.", ex);
            }

            // Dependency injections 
            services.AddScoped<TodoService>();
            services.AddScoped<UserService>();
            services.AddScoped<ITodoRepository, TodoRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // JWT configuration
            var jwtSettings = new JwtSettings
            {
                SecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? Configuration["JwtSettings:SecretKey"],
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? Configuration["JwtSettings:Issuer"],
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? Configuration["JwtSettings:Audience"]
            };

            services.AddSingleton(jwtSettings);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                    };
                });

            // Configure AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                // Add your mapping profiles here
                cfg.AddProfile<MappingProfile>();
            });

            services.AddSingleton(mapperConfig.CreateMapper());

            services.AddSwaggerGen();

            services.AddEndpointsApiExplorer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
