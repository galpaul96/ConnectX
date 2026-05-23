using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ConnectX.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
}
internal class EfContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    private const string ApplicationName = "ConnectX.Ef";

    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        string connectionString = args[0] ?? "Host=localhost;Port=5432;Database=ConnectX.Web;Username=postgres;Password=postgres";

        string applicationConnectionString = $"Application Name={ApplicationName};{connectionString}";

        optionsBuilder.UseNpgsql(applicationConnectionString, x =>
        {
            x.EnableRetryOnFailure();
        });

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
