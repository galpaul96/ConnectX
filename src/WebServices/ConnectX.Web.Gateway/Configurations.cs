using ConnectX.Web.Gateway.ExternalApis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ConnectX.Web.Gateway
{
    public static class Configurations
    {
        public static void ConfiugreGateway(this IServiceCollection services)
        {
            services.TryAddScoped<IGatewayClient, GatewayClient>();
            //services.AddHttpClient("foo"); // adding an HttpClient named "foo" with a default configuration

            services.AddHttpClient("api", c => c.BaseAddress = new Uri("https://www.example.com")) // configuring HttpClient itself
                //.AddHttpMessageHandler<MyAuthHandler>() // adding additional delegating handlers to form a message handler chain
                .ConfigurePrimaryHttpMessageHandler(b => new HttpClientHandler() { AllowAutoRedirect = false }) // configuring primary handler
                .SetHandlerLifetime(TimeSpan.FromMinutes(30)); // changing the handler recycling interval
        }
    }
}
