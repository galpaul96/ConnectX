using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace ConnectX.Api.Ef
{
    internal class EfContext : DbContext
    {
        public EfContext(DbContextOptions<EfContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(EfContext))!);
        }
    }

    internal class EfContextFactory : IDesignTimeDbContextFactory<EfContext>
    {
        private const string ApplicationName = "ConnectX.Ef";

        public EfContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EfContext>();
            string connectionString = args[0] ?? "Host=localhost;Port=5432;Database=ConnectX.Web;Username=postgres;Password=postgres";

            string applicationConnectionString = $"Application Name={ApplicationName};{connectionString}";

            optionsBuilder.UseNpgsql(applicationConnectionString, x =>
            {
                x.EnableRetryOnFailure();
            });

            return new EfContext(optionsBuilder.Options);
        }
    }
}
