using System.Text;
using AuthService.Application.Ports.Repositories;
using AuthService.Application.Ports.Services;
using AuthService.Application.UseCases.LoginUseCase;
using AuthService.Application.UseCases.RefreshTokenUseCase;
using AuthService.Domain;
using AuthService.Infrastructure.Options;
using AuthService.Infrastructure.Repositories;
using AuthService.Infrastructure.Services;
using AuthService.Infrustructure.Migrations;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using PasswordOptions = Microsoft.AspNetCore.Identity.PasswordOptions;

namespace AuthService.Grpc.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBearerAuthentication(this IServiceCollection sc, IConfiguration configuration)
    {
        sc.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtTokenOptions:Key"])),
                ClockSkew = TimeSpan.Zero
            };
        });

        sc.AddAuthorization();

        return sc;
    }

    public static IServiceCollection AddOptions(this IServiceCollection sc, IConfiguration configuration)
    {
        sc.Configure<JwtTokenOptions>(configuration.GetSection("JwtTokenOptions"));
        sc.Configure<PasswordOptions>(configuration.GetSection("PasswordOptions"));
        sc.Configure<DatabaseOptions>(configuration.GetSection("DatabaseOptions"));
        
        return sc;
    }

    public static IServiceCollection AddDatabaseConnection(this IServiceCollection sc)
    {
        sc.AddScoped<NpgsqlDataSource>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<DatabaseOptions>>();
            var builder = new NpgsqlDataSourceBuilder(options.Value.ConnectionString);
            builder.MapEnum<Role>(pgName: "role");
            return builder.Build();
        });

        return sc;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection sc)
    {
        sc.AddScoped<IUserRepository, UserRepository>();
        sc.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        return sc;
    }

    public static IServiceCollection AddServices(this IServiceCollection sc)
    {
        sc.AddScoped<IAuthTokenService, AuthTokenService>();
        sc.AddScoped<IPasswordCoder, PasswordCoder>();
        return sc;
    }
    
    public static IServiceCollection AddUseCases(this IServiceCollection sc)
    {
        sc.AddScoped<ILoginUseCase, LoginUseCase>();
        sc.AddScoped<IRefreshTokenUseCase, RefreshTokenUseCase>();
        
        return sc;
    }
    
    public static IServiceCollection AddDatabaseMigrations(
        this IServiceCollection sc)
    {
        sc
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(provider =>
                {
                    var options = provider.GetRequiredService<IOptions<DatabaseOptions>>();
                    return options.Value.ConnectionString;
                })
                .WithMigrationsIn(typeof(InitialMigration).Assembly));

        return sc;
    }
}