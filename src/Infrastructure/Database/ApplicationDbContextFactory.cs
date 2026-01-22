using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Database;
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        string connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Database")
            ?? "Host=localhost;Port=5432;Database=salesdb;Username=postgres;Password=postgres";

        optionsBuilder
            .UseNpgsql(connectionString, npgsqlOptions =>
                npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default))
            .UseSnakeCaseNamingConvention();

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
