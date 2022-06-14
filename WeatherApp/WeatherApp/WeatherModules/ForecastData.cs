using System.Collections.Generic;

namespace WeatherApp.WeatherModules
{
    public class ForecastData
    {
        public List<WeatherDayData> WeatherDayDataList { get; set; } = new ();
    }
}