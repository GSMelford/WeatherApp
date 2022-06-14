using Newtonsoft.Json;

namespace WeatherApp.WeatherProviders.OpenWeather.Entities.Current; 

public class Wind
{
    [JsonProperty("speed")]
    public double Speed { get; set; }

    [JsonProperty("deg")]
    public int Deg { get; set; }

    [JsonProperty("gust")]
    public double Gust { get; set; }
}