using ConnectX.Web.Gateway;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ConnectX.Web.Services
{
    public static class Configurations
    {
        public static void ConfiugreServices(this IServiceCollection services)
        {
            services.ConfiugreGateway();

            services.TryAddScoped<IWeatherService, WeatherService>();
        }
    }
}
