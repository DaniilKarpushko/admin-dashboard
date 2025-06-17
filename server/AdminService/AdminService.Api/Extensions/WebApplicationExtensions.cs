using FluentMigrator.Runner;

namespace AdminService.Api.Extensions;

public static class WebApplicationExtensions
{
    public static IHost ApplyMigrations(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
        return host;
    }
}