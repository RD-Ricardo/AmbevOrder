using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AmbevOrder.ProcessOrder.Data
{
    public class AmbevOrderProcessOrderDbContextFactory : IDesignTimeDbContextFactory<AmbevOrderProcessOrderDbContext>
    {
        public AmbevOrderProcessOrderDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AmbevOrderProcessOrderDbContext>();

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            optionsBuilder.UseNpgsql(configuration["ConnectionDatabase"]);
           
            return new AmbevOrderProcessOrderDbContext(optionsBuilder.Options);
        }
    }
}
