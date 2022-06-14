using System.Threading.Tasks;
using WeatherApp.Interfaces;
using WeatherApp.Modules;
using WeatherApp.WeatherModules;
using WeatherApp.WeatherProviders;
using WeatherApp.WeatherProviders.OpenWeather;
using WeatherApp.WeatherProviders.WeatherApi;

namespace WeatherApp;

public class WeatherUpdater
{
    private readonly WeatherInformation _weatherInformation;
    private WeatherData _weatherData = new ();
    private ForecastData _forecastData = new ();

    public WeatherUpdater(WeatherInformation weatherInformation)
    {
        _weatherInformation = weatherInformation;
    }

    private static IWeatherService ChooseService(int serviceIndex)
    {
        return serviceIndex switch
        {
            0 => new AllProviders(),
            1 => new WeatherApiService(),
            2 => new OpenWeatherService(),
            _ => null
        };
    }
    
    public async Task SetCurrentWeather(int serviceIndex, string location)
    {
        _weatherData = await ChooseService(serviceIndex).GetCurrent(location);
    }

    public async Task<ForecastData> GetForecast(int serviceIndex, string location, int days)
    {
        _forecastData = await ChooseService(serviceIndex).GetForecast(location, days);
        return _forecastData;
    }
    
    public void UpdateWeatherInfo(int degreeProviderPicker)
    {
        _weatherInformation.Location = _weatherData.Location;
        _weatherInformation.Temperature =
            $"{(degreeProviderPicker == 0 ? _weatherData.TemperatureC : _weatherData.TemperatureF)}°";
        _weatherInformation.TemperatureFeelsLike =
            $"feels like {(degreeProviderPicker == 0 ? _weatherData.TemperatureFeelsLikeC : _weatherData.TemperatureFeelsLikeF)}°";
        _weatherInformation.WeatherStatus = _weatherData.WeatherStatus;

        _weatherInformation.LastUpdatedTime = $"Latest updates: {_weatherData.Date}";
        _weatherInformation.WindSpeed = $"Wind speed: {_weatherData.WindSpeed} m/s";
        _weatherInformation.WindDirect = $"Direction of the wind: {_weatherData.WindDirect}";
        _weatherInformation.Pressure = $"Pressure: {_weatherData.Pressure}";
    }
    
    
    public string GetWeatherIcon()
    {
        return _weatherData.WeatherIconUrl;
    }
}