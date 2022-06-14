using Newtonsoft.Json;

namespace WeatherApp.WeatherProviders.WeatherApi.Entities.Forecast
{
    public class ForecastRoot
    {
        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("current")]
        public Current Current { get; set; }

        [JsonProperty("forecast")]
        public Forecast Forecast { get; set; }
    }
}

