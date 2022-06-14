using Newtonsoft.Json;

namespace WeatherApp.WeatherProviders.OpenWeather.Entities.Forecast; 

public class Coord
{
    [JsonProperty("lat")]
    public double Lat { get; set; }

    [JsonProperty("lon")]
    public double Lon { get; set; }
}