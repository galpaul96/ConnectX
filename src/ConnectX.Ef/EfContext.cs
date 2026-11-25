using ConnectX.Domain.Entities.Catalogs.Spells;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;
namespace ConnectX.Ef
{
    internal class EfContext : DbContext
    {
        public EfContext(DbContextOptions<EfContext> options)
            : base(options) { }

        public DbSet<Spell> Spells;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(EfContext))!);
        }
    }

    internal class EfContextFactory : IDesignTimeDbContextFactory<EfContext>
    {
        private const string ApplicationName = "ConnectX";

        public EfContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EfContext>();
            string connectionString;
            if (args != null && args.Any())
            {
                connectionString = args[0];
            }
            else
            {
                connectionString = "Host=localhost;Port=5432;Database=ConnectX.Data;Username=postgres;Password=postgres";
            }
            string applicationConnectionString = $"Application Name={ApplicationName};{connectionString}";

            optionsBuilder.UseNpgsql(applicationConnectionString, x =>
            {
                x.EnableRetryOnFailure();
            });

            return new EfContext(optionsBuilder.Options);
        }
    }
}
