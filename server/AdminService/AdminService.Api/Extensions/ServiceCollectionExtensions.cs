using System.Text;
using AdminService.Application.Ports.Repositories;
using AdminService.Application.Services.ClientService;
using AdminService.Application.Services.PaymentService;
using AdminService.Application.Services.RateService;
using AdminService.Infrastructure;
using AdminService.Infrastructure.Migrations;
using AdminService.Infrastructure.Options;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

namespace AdminService.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOptions(this IServiceCollection sc, IConfiguration configuration)
    {
        sc.Configure<DatabaseOptions>(configuration.GetSection("DatabaseOptions"));

        return sc;
    }

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

    public static IServiceCollection AddDatabaseConnection(this IServiceCollection sc)
    {
        sc.AddScoped(provider =>
        {
            var options = provider.GetRequiredService<IOptions<DatabaseOptions>>();
            var builder = new NpgsqlDataSourceBuilder(options.Value.ConnectionString);
            return builder.Build();
        });

        return sc;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection sc)
    {
        sc.AddScoped<IClientRepository, ClientRepository>();
        sc.AddScoped<IRateHistoryRepository, RateHistoryRepository>();
        sc.AddScoped<IPaymentRepository, PaymentRepository>();

        return sc;
    }

    public static IServiceCollection AddServices(this IServiceCollection sc)
    {
        sc.AddScoped<IClientService, ClientService>();
        sc.AddScoped<IRateService, RateService>();
        sc.AddScoped<IPaymentService, PaymentService>();

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