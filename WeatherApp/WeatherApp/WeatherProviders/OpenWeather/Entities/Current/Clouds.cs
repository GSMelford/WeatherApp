using Newtonsoft.Json;

namespace WeatherApp.WeatherProviders.OpenWeather.Entities.Current; 

public class Clouds
{
    [JsonProperty("all")]
    public int All { get; set; }
}