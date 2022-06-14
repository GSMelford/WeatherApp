using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApp.Extensions;
using WeatherApp.Interfaces;
using WeatherApp.Requests;
using WeatherApp.WeatherModules;
using WeatherApp.WeatherProviders.WeatherApi.Entities.Current;
using WeatherApp.WeatherProviders.WeatherApi.Entities.Forecast;

namespace WeatherApp.WeatherProviders.WeatherApi;

public class WeatherApiService : IWeatherService
{
    private const string VERSION = "v1";
    private const string API_KEY = "1d20f3904cfe444c84883650221206";
    private static readonly string BaseUrl = $"https://api.weatherapi.com/{VERSION}/";
        
    public async Task<WeatherData> GetCurrent(string location)
    {
        const string method = "current.json";
        HttpRequestMessage httpRequestMessage = new RequestBuilder(HttpMethod.Get, BaseUrl + method)
            .AddParameters(new Dictionary<string, object>
            {
                {"key", API_KEY},
                {"q", location},
                {"aqi", "no"}
            }).Build();

        HttpResponseMessage httpResponseMessage = await RequestSender.DoRequest(httpRequestMessage);
        return !httpResponseMessage.IsSuccessStatusCode 
            ? new WeatherData() 
            : MapToWhetherData((await httpResponseMessage.Content.ReadAsStringAsync()).ToObject<Entities.Current.CurrentRoot>());
    }

    private static WeatherData MapToWhetherData(Entities.Current.CurrentRoot current)
    {
        return new WeatherData
        {
            Location = $"{current.Location.Country}, {current.Location.Region}\n{current.Location.Name}",
            WeatherStatus = current.Current.Condition.Text,
            WeatherIconUrl = $"http:{current.Current.Condition.Icon}",
            Pressure = current.Current.PressureIn.ToString(CultureInfo.InvariantCulture),
            TemperatureC = ((int) current.Current.TempC).ToString(CultureInfo.InvariantCulture),
            TemperatureF = ((int) current.Current.TempF).ToString(CultureInfo.InvariantCulture),
            WindDirect = current.Current.WindDir,
            WindSpeed = current.Current.WindMph.ToString(CultureInfo.InvariantCulture),
            TemperatureFeelsLikeC = ((int) current.Current.FeelslikeC).ToString(CultureInfo.InvariantCulture),
            TemperatureFeelsLikeF = ((int) current.Current.FeelslikeF).ToString(CultureInfo.InvariantCulture),
            Date = current.Current.LastUpdated.ToString("t")
        };
    }
        
    public async Task<ForecastData> GetForecast(string location, int days)
    {
        const string method = "forecast.json";
        HttpRequestMessage httpRequestMessage = new RequestBuilder(HttpMethod.Get, BaseUrl + method)
            .AddParameters(new Dictionary<string, object>
            {
                {"key", API_KEY},
                {"q", location},
                {"days", days},
                {"alerts", "no"},
                {"aqi", "no"}
            }).Build();

        HttpResponseMessage httpResponseMessage = await RequestSender.DoRequest(httpRequestMessage);
        return !httpResponseMessage.IsSuccessStatusCode 
            ? new ForecastData() 
            : MapToForecast((await httpResponseMessage.Content.ReadAsStringAsync()).ToObject<Entities.Forecast.ForecastRoot>());
    }

    private static ForecastData MapToForecast(ForecastRoot forecastRoot)
    {
        ForecastData forecastData = new ForecastData();
            
        foreach (Forecastday forecastDay in forecastRoot.Forecast.Forecastday)
        {
            WeatherDayData weatherDayData = new WeatherDayData
            {
                Date = forecastDay.Date,
                TemperatureMaxC = ((int) forecastDay.Day.MaxtempC).ToString(CultureInfo.InvariantCulture),
                TemperatureMaxF = ((int) forecastDay.Day.MaxtempF).ToString(CultureInfo.InvariantCulture),
                TemperatureMinC = ((int) forecastDay.Day.MintempC).ToString(CultureInfo.InvariantCulture),
                TemperatureMinF = ((int) forecastDay.Day.MintempF).ToString(CultureInfo.InvariantCulture),
                AverageTemperatureC = ((int) forecastDay.Hour.Sum(x => x.TempC) / forecastDay.Hour.Count).ToString(CultureInfo.InvariantCulture),
                AverageTemperatureF = ((int) forecastDay.Hour.Sum(x => x.TempF) / forecastDay.Hour.Count).ToString(CultureInfo.InvariantCulture),
                WeatherStatus = forecastDay.Day.Condition.Text,
                WeatherIconUrl = $"http:{forecastDay.Day.Condition.Icon}"
            };
                
            foreach (Hour hour in forecastDay.Hour)
            {
                weatherDayData.WeatherDataHours.Add(new WeatherData
                {
                    Date = hour.Time,
                    Location = string.Join(" ", forecastRoot.Location.Country, forecastRoot.Location.Region, forecastRoot.Location.Name),
                    Pressure = hour.PressureIn.ToString(CultureInfo.InvariantCulture),
                    TemperatureC = ((int) hour.TempC).ToString(CultureInfo.InvariantCulture),
                    TemperatureF = ((int) hour.TempF).ToString(CultureInfo.InvariantCulture),
                    WeatherStatus = hour.Condition.Text,
                    WeatherIconUrl = $"http:{hour.Condition.Icon}",
                    WindDirect = hour.WindDir,
                    WindSpeed = hour.WindMph.ToString(CultureInfo.InvariantCulture),
                    TemperatureFeelsLikeC = ((int) hour.FeelslikeC).ToString(CultureInfo.InvariantCulture),
                    TemperatureFeelsLikeF = ((int) hour.FeelslikeF).ToString(CultureInfo.InvariantCulture)
                });
            }
                
            forecastData.WeatherDayDataList.Add(weatherDayData);
        }

        return forecastData;
    }
}