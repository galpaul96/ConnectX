using ConnectX.Api.MongoDb.Students;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ConnectX.Api.MongoDb;

public static class Configurations
{
    public static void ConfigureMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbOptions>(configuration.Bind);

        services.TryAddSingleton<IMongoClient>(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<MongoDbOptions>>().Value;

            return new MongoClient(options.GetConnectionString());
        });

        services.TryAddSingleton(serviceProvider =>
        {
            var client = serviceProvider.GetRequiredService<IMongoClient>();
            var options = serviceProvider.GetRequiredService<IOptions<MongoDbOptions>>().Value;

            return client.GetDatabase(options.DatabaseName);
        });

        services.TryAddScoped<IStudentRepository, StudentRepository>();
    }
}
