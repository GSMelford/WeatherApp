using Newtonsoft.Json;

namespace WeatherApp.WeatherProviders.WeatherApi.Entities.Current
{
    public class Condition
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }
    }
}

