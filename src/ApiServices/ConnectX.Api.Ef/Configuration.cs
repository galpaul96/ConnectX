using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ConnectX.Api.Ef
{
    public static class Configuration
    {
        public static void ConfigureRepository(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.TryAddScoped<IRepository, Repository>();

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (environment == "InMemory")
            {
                services.AddDbContext<EfContext>(
                                    options =>
                                    options.UseInMemoryDatabase("ConnectX")
                                        );
            }
            else
            {
                var connectionString = Environment.GetEnvironmentVariable("PostgreConnection");

                services.AddDbContext<EfContext>(
                options =>
                    options.UseNpgsql(
                        connectionString,
                       x => x.MigrationsAssembly("ConnectX.EF"))
                    );
            }
        }
    }
}
