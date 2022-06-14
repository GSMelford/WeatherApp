using Newtonsoft.Json;

namespace WeatherApp.WeatherProviders.OpenWeather.Entities.Forecast; 

public class Rain
{
    [JsonProperty("3h")]
    public double _3h { get; set; }
}