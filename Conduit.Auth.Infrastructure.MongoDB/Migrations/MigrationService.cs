using System.Threading.Tasks;
using FluentMigrator.Runner;

namespace Conduit.Auth.Infrastructure.MongoDB.Migrations;

public class MigrationService
{
    private readonly IMigrationRunner _runner;

    public MigrationService(
        IMigrationRunner runner)
    {
        _runner = runner;
    }

    public Task InitializeAsync()
    {
        RunMigrations();

        return Task.CompletedTask;
    }

    private void RunMigrations()
    {
        _runner.MigrateUp();
    }
}
