using Newtonsoft.Json;

namespace WeatherApp.WeatherProviders.WeatherApi.Entities.Current
{
    public class CurrentRoot
    {
        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("current")]
        public Current Current { get; set; }
    }
}

