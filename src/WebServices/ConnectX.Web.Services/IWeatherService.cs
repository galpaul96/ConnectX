using ConnectX.Domain;

namespace ConnectX.Web.Services
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherAsync();
    }
}
