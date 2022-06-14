using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeatherApp.WeatherProviders.OpenWeather.Entities.Forecast; 

public class ForecastRoot
{
    [JsonProperty("cod")]
    public string Cod { get; set; }

    [JsonProperty("message")]
    public int Message { get; set; }

    [JsonProperty("cnt")]
    public int Cnt { get; set; }

    [JsonProperty("list")]
    public List<HourWeather> ListObjects { get; set; }

    [JsonProperty("city")]
    public City City { get; set; }
}