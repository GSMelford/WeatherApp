using Newtonsoft.Json;

namespace WeatherApp.WeatherProviders.OpenWeather.Entities.Location;

public class LocationRoot
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("lat")]
    public double Lat { get; set; }

    [JsonProperty("lon")]
    public double Lon { get; set; }

    [JsonProperty("country")]
    public string Country { get; set; }

    [JsonProperty("state")]
    public string State { get; set; }
}