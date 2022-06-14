using Newtonsoft.Json;

namespace WeatherApp.WeatherProviders.OpenWeather.Entities.Forecast; 

public class Clouds
{
    [JsonProperty("all")]
    public int All { get; set; }
}