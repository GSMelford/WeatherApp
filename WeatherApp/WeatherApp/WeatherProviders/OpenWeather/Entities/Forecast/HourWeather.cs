using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeatherApp.WeatherProviders.OpenWeather.Entities.Forecast; 

public class HourWeather
{
    [JsonProperty("dt")]
    public int Dt { get; set; }

    [JsonProperty("main")]
    public Main Main { get; set; }

    [JsonProperty("weather")]
    public List<Weather> Weather { get; set; }

    [JsonProperty("clouds")]
    public Clouds Clouds { get; set; }

    [JsonProperty("wind")]
    public Wind Wind { get; set; }

    [JsonProperty("visibility")]
    public int Visibility { get; set; }

    [JsonProperty("pop")]
    public double Pop { get; set; }

    [JsonProperty("rain")]
    public Rain Rain { get; set; }

    [JsonProperty("sys")]
    public Sys Sys { get; set; }

    [JsonProperty("dt_txt")]
    public string DtTxt { get; set; }
}