using Newtonsoft.Json;

namespace WeatherApp.WeatherProviders.OpenWeather.Entities.Current; 

public class Coord
{
    [JsonProperty("lon")]
    public double Lon { get; set; }

    [JsonProperty("lat")]
    public double Lat { get; set; }
}