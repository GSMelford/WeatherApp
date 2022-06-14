using Newtonsoft.Json;

namespace WeatherApp.WeatherProviders.OpenWeather.Entities.Current; 

public class Rain
{
    [JsonProperty("1h")]
    public double _1h { get; set; }
}