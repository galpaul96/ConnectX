using ConnectX.Web.Components;
using ConnectX.Web.Components.Admin;
using ConnectX.Web.Components.Account;
using ConnectX.Web.Data;
using ConnectX.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Internal;
using System.Diagnostics;

namespace ConnectX.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();
        builder.AddRedisOutputCache("redis-cache");

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            // Allow metadata for a multi-file selection; file streams retain their own 1 MB limit.
            .AddHubOptions(options => options.MaximumReceiveMessageSize = 1024 * 1024);

        builder.Services.AddCascadingAuthenticationState();
        ConnectX.Ef.Configuration.ConfigureRepository(builder.Services, builder.Configuration);
        builder.Services.AddSpellServices();
        builder.Services.AddItemServices();
        builder.Services.AddScoped<ConnectX.Web.Components.Pages.DND.Catalogs.Items.ItemUploadService>();
        builder.Services.AddScoped<ConnectX.Web.Components.Pages.DND.Catalogs.Skills.SpellUploadService>();
        builder.Services.AddAuthorization();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<IdentityUserAccessor>();
        builder.Services.AddScoped<IdentityAdminService>();
        builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

        var connectionString = builder.Configuration.GetConnectionString("web-identity") ?? throw new InvalidOperationException("Connection string 'web-identity' not found.");

        static void ConfigureWebDbContext(DbContextOptionsBuilder options, string connectionString)
        {
            options.UseNpgsql(
                connectionString,
                x => x.MigrationsAssembly("ConnectX.Web"));
        }

        builder.Services.AddDbContext<ApplicationDbContext>(
            options => ConfigureWebDbContext(options, connectionString));

        var sw = Stopwatch.StartNew();
        Console.WriteLine("Deploying database...");
        var dbContext = new EfContextFactory().CreateDbContext([connectionString]);
        dbContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
        dbContext.Database.Migrate();
        Console.WriteLine($"Deployment done in {sw.Elapsed}");

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.UseOutputCache();
        // Add additional endpoints required by the Identity /Account Razor components.
        app.MapAdditionalIdentityEndpoints();

        app.Run();
    }
}

internal class EfContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    private const string ApplicationName = "ConnectX.Identity";

    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        string connectionString = args.Length > 0
            ? args[0]
            : "Host=localhost;Port=5432;Database=ConnectX.Web;Username=postgres;Password=postgres";

        string applicationConnectionString = $"Application Name={ApplicationName};{connectionString}";

        optionsBuilder.UseNpgsql(applicationConnectionString, x =>
        {
            x.EnableRetryOnFailure();
        });

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
