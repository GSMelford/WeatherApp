using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using WeatherApp.Extensions;
using WeatherApp.Interfaces;
using WeatherApp.WeatherModules;
using WeatherApp.WeatherProviders.OpenWeather;
using WeatherApp.WeatherProviders.WeatherApi;

namespace WeatherApp.WeatherProviders;

public class AllProviders : IWeatherService
{
    public async Task<WeatherData> GetCurrent(string location)
    {
        WeatherData weatherDataWeatherApi = await new WeatherApiService().GetCurrent(location);
        WeatherData weatherDataOpenWeather = await new OpenWeatherService().GetCurrent(location);
        
        return MergeWeathers(weatherDataWeatherApi, weatherDataOpenWeather);
    }

    public async Task<ForecastData> GetForecast(string location, int days)
    {
        ForecastData forecastDataWeatherApi = await new WeatherApiService().GetForecast(location, days);
        ForecastData forecastDataOpenWeather = await new OpenWeatherService().GetForecast(location, days);

        ForecastData forecastData = new ForecastData
        {
            WeatherDayDataList = new List<WeatherDayData>()
        };
        
        for (int i = 0; i < forecastDataWeatherApi.WeatherDayDataList.Count; i++)
        {
            WeatherDayData weatherDayData = new WeatherDayData
            {
                Date = forecastDataWeatherApi.WeatherDayDataList[i].Date,
                WeatherStatus = forecastDataOpenWeather.WeatherDayDataList[i].WeatherStatus,
                AverageTemperatureC =
                    ((forecastDataWeatherApi.WeatherDayDataList[i].AverageTemperatureC.ToInt()
                      + forecastDataWeatherApi.WeatherDayDataList[i].AverageTemperatureC.ToInt()) / 2).ToString(),
                AverageTemperatureF =
                    ((forecastDataWeatherApi.WeatherDayDataList[i].AverageTemperatureF.ToInt()
                      + forecastDataWeatherApi.WeatherDayDataList[i].AverageTemperatureF.ToInt()) / 2).ToString(),
                TemperatureMaxC =
                    ((forecastDataWeatherApi.WeatherDayDataList[i].TemperatureMaxC.ToInt()
                      + forecastDataWeatherApi.WeatherDayDataList[i].TemperatureMaxC.ToInt()) / 2).ToString(),
                TemperatureMaxF =
                    ((forecastDataWeatherApi.WeatherDayDataList[i].TemperatureMaxF.ToInt()
                      + forecastDataWeatherApi.WeatherDayDataList[i].TemperatureMaxF.ToInt()) / 2).ToString(),
                TemperatureMinC =
                    ((forecastDataWeatherApi.WeatherDayDataList[i].TemperatureMinC.ToInt()
                      + forecastDataWeatherApi.WeatherDayDataList[i].TemperatureMinC.ToInt()) / 2).ToString(),
                TemperatureMinF =
                    ((forecastDataWeatherApi.WeatherDayDataList[i].TemperatureMinF.ToInt()
                      + forecastDataWeatherApi.WeatherDayDataList[i].TemperatureMinF.ToInt()) / 2).ToString(),
                WeatherIconUrl = forecastDataWeatherApi.WeatherDayDataList[i].WeatherIconUrl
            };
            
            for (int j = 0; j < forecastDataOpenWeather.WeatherDayDataList[i].WeatherDataHours.Count; j++)
            {
                weatherDayData.WeatherDataHours.Add(
                    MergeWeathers(forecastDataWeatherApi.WeatherDayDataList[i].WeatherDataHours[j * 3], 
                        forecastDataOpenWeather.WeatherDayDataList[i].WeatherDataHours[j]));
            }
            
            forecastData.WeatherDayDataList.Add(weatherDayData);
        }

        return forecastData;
    }

    private WeatherData MergeWeathers(WeatherData firstWeatherData, WeatherData secondWeatherData)
    {
        return new WeatherData
        {
            Date = firstWeatherData.Date,
            Location = firstWeatherData.Location,
            Pressure =
                ((firstWeatherData.Pressure.ToInt() + secondWeatherData.Pressure.ToInt()) / 2).ToString(),
            TemperatureC =
                ((firstWeatherData.TemperatureC.ToInt() + secondWeatherData.TemperatureC.ToInt()) / 2)
                .ToString(),
            TemperatureF =
                ((firstWeatherData.TemperatureF.ToInt() + secondWeatherData.TemperatureF.ToInt()) / 2)
                .ToString(),
            WindSpeed = ((firstWeatherData.WindSpeed.ToDouble() + secondWeatherData.WindSpeed.ToDouble()) / 2)
                .ToString(CultureInfo.InvariantCulture),
            WeatherStatus = secondWeatherData.WeatherStatus,
            WindDirect = firstWeatherData.WindDirect,
            WeatherIconUrl = firstWeatherData.WeatherIconUrl,
            TemperatureFeelsLikeC =
                ((firstWeatherData.TemperatureFeelsLikeC.ToInt() +
                  secondWeatherData.TemperatureFeelsLikeC.ToInt()) / 2).ToString(),
            TemperatureFeelsLikeF =
                ((firstWeatherData.TemperatureFeelsLikeC.ToInt() +
                  secondWeatherData.TemperatureFeelsLikeC.ToInt()) / 2).ToString()
        };
    }
}