using System;

namespace WeatherApp.WeatherModules
{
    public class WeatherData
    {
        public string Location { get; set; }
        public string Date { get; set; }
        public string TemperatureC { get; set; }
        public string TemperatureF { get; set; }
        public string TemperatureFeelsLikeC { get; set; }
        public string TemperatureFeelsLikeF { get; set; }
        public string WindSpeed { get; set; }
        public string WindDirect { get; set; }
        public string Pressure { get; set; }
        public string WeatherStatus { get; set; }
        public string WeatherIconUrl { get; set; }
    }
}