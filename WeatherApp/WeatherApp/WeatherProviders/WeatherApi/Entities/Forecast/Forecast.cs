using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeatherApp.WeatherProviders.WeatherApi.Entities.Forecast
{
    public class Forecast
    {
        [JsonProperty("forecastday")]
        public List<Forecastday> Forecastday { get; set; }
    }
}

