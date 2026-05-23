using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics;

namespace ConnectX.Api.Ef
{
    public static class Configuration
    {
        public static void ConfigureRepository(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.TryAddScoped<IRepository, Repository>();

            var connectionString = Environment.GetEnvironmentVariable("ApiDatabase") ?? throw new InvalidOperationException("Connection string 'WebDatabase' not found.");

            services.AddDbContext<EfContext>(
            options =>
                options.UseNpgsql(
                    connectionString,
                   x => x.MigrationsAssembly("ConnectX.EF"))
                );

            var sw = Stopwatch.StartNew();
            Console.WriteLine("Deploying database...");
            var dbContext = new EfContextFactory().CreateDbContext([connectionString]);
            dbContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
            dbContext.Database.Migrate();
            Console.WriteLine($"Deployment done in {sw.Elapsed}");
        }
    }
}
