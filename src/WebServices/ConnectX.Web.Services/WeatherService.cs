using ConnectX.Domain;
using ConnectX.Web.Gateway.ExternalApis;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ConnectX.Web.Services
{
    internal class WeatherService : IWeatherService
    {
        private readonly ILogger<WeatherService> logger;
        private readonly IGatewayClient gatewayClient;

        public WeatherService(ILogger<WeatherService> logger, IGatewayClient gatewayClient)
        {
            this.logger = logger;
            this.gatewayClient = gatewayClient;
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherAsync()
        {
            logger.LogInformation("Getting weather forecast from the gateway client.");
            var response = await gatewayClient.GetAsync("http://api:8080", "WeatherForecast");

            if (response.IsSuccessStatusCode)
            {
                var weatherForecast = await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
                logger.LogInformation("Successfully retrieved weather forecast.");
                return weatherForecast!;
            }
            else
            {
                logger.LogError("Failed to retrieve weather forecast. Status code: {StatusCode}", response.StatusCode);
                throw new Exception($"Failed to retrieve weather forecast. Status code: {response.StatusCode}");
            }
        }
    }
}
