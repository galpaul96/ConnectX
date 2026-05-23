using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ConnectX.Api.Ef;

namespace ConnectX.Api.Services
{
    public static class Configurations
    {
        public static void ConfiugreGateway(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.ConfigureRepository(configuration);
            //services.AddHttpClient("foo"); // adding an HttpClient named "foo" with a default configuration

            //services.AddHttpClient("api", c => c.BaseAddress = new Uri("https://www.example.com")) // configuring HttpClient itself
            //    //.AddHttpMessageHandler<MyAuthHandler>() // adding additional delegating handlers to form a message handler chain
            //    .ConfigurePrimaryHttpMessageHandler(b => new HttpClientHandler() { AllowAutoRedirect = false }) // configuring primary handler
            //    .SetHandlerLifetime(TimeSpan.FromMinutes(30)); // changing the handler recycling interval
        }
    }
}
