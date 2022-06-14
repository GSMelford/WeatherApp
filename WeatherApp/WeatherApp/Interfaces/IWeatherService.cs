using System.Threading.Tasks;
using WeatherApp.WeatherModules;

namespace WeatherApp.Interfaces;

public interface IWeatherService
{
    Task<WeatherData> GetCurrent(string location);
    Task<ForecastData> GetForecast(string location, int days);
}