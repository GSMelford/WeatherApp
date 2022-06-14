using Newtonsoft.Json;

namespace WeatherApp.WeatherProviders.OpenWeather.Entities.Forecast; 

public class Sys
{
    [JsonProperty("pod")]
    public string Pod { get; set; }
}