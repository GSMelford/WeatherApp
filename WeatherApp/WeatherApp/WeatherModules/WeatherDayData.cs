using System.Collections.Generic;

namespace WeatherApp.WeatherModules
{
    public class WeatherDayData
    {
        public string Date { get; set; }
        public string TemperatureMaxC { get; set; }
        public string TemperatureMinC { get; set; }
        public string TemperatureMaxF { get; set; }
        public string TemperatureMinF { get; set; }
        public string AverageTemperatureC { get; set; }
        public string AverageTemperatureF { get; set; }
        public string WeatherStatus { get; set; }
        public string WeatherIconUrl { get; set; }
        public List<WeatherData> WeatherDataHours { get; set; } = new ();
    }
}