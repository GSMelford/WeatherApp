using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApp.Extensions;
using WeatherApp.Interfaces;
using WeatherApp.Requests;
using WeatherApp.WeatherModules;
using WeatherApp.WeatherProviders.OpenWeather.Entities.Forecast;
using WeatherApp.WeatherProviders.OpenWeather.Entities.Location;

namespace WeatherApp.WeatherProviders.OpenWeather;

public class OpenWeatherService : IWeatherService
{
    private const string API_KEY = "e61133d6f77b17871da77c5364cdb3ca";
    private const string BASE_URL = "https://api.openweathermap.org/";
    
    public async Task<WeatherData> GetCurrent(string location)
    {
        LocationRoot locationRoot = await GetLocation(location);

        if (locationRoot is null)
        {
            return new WeatherData();
        }
        
        const string method = "data/2.5/weather";
        HttpRequestMessage httpRequestMessage = new RequestBuilder(HttpMethod.Get, BASE_URL + method)
            .AddParameters(new Dictionary<string, object>
            {
                {"lat", locationRoot.Lat},
                {"lon", locationRoot.Lon},
                {"appid", API_KEY}
            }).Build();

        HttpResponseMessage httpResponseMessage = await RequestSender.DoRequest(httpRequestMessage);
        return !httpResponseMessage.IsSuccessStatusCode 
            ? new WeatherData()
            : MapToWhetherData((await httpResponseMessage.Content.ReadAsStringAsync()).ToObject<Entities.Current.CurrentRoot>(), locationRoot);
    }

    private static WeatherData MapToWhetherData(Entities.Current.CurrentRoot current, LocationRoot locationRoot)
    {
        return new WeatherData
        {
            Location = $"{locationRoot.Country}, {locationRoot.State}\n{locationRoot.Name}",
            WeatherStatus = current.Weather.FirstOrDefault()?.Main,
            WeatherIconUrl = $"http://openweathermap.org/img/wn/{current.Weather.FirstOrDefault()?.Icon}@2x.png",
            Pressure = current.Main.Pressure.ToString(),
            TemperatureC = ((int) (current.Main.Temp - 273.15)).ToString(CultureInfo.InvariantCulture),
            TemperatureF = ((int) (current.Main.Temp - 273.15 + 32)).ToString(CultureInfo.InvariantCulture),
            WindDirect = string.Empty,
            WindSpeed = current.Wind.Speed.ToString(CultureInfo.InvariantCulture),
            TemperatureFeelsLikeC = ((int) (current.Main.FeelsLike - 273.15)).ToString(CultureInfo.InvariantCulture),
            TemperatureFeelsLikeF = ((int) (current.Main.FeelsLike - 273.15 + 32)).ToString(CultureInfo.InvariantCulture),
            Date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(current.Dt).ToLocalTime().ToString("t")
        };
    }
    
    public async Task<ForecastData> GetForecast(string location, int days)
    {
        LocationRoot locationRoot = await GetLocation(location);
        
        if (locationRoot is null)
        {
            return new ForecastData();
        }
        
        const string method = "data/2.5/forecast";
        HttpRequestMessage httpRequestMessage = new RequestBuilder(HttpMethod.Get, BASE_URL + method)
            .AddParameters(new Dictionary<string, object>
            {
                {"lat", locationRoot.Lat},
                {"lon", locationRoot.Lon},
                {"appid", API_KEY}
            }).Build();

        HttpResponseMessage httpResponseMessage = await RequestSender.DoRequest(httpRequestMessage);
        return !httpResponseMessage.IsSuccessStatusCode 
            ? new ForecastData()
            : MapToForecast((await httpResponseMessage.Content.ReadAsStringAsync()).ToObject<ForecastRoot>(), locationRoot);
    }

    private async Task<LocationRoot> GetLocation(string location)
    {
        const string method = "geo/1.0/direct";
        HttpRequestMessage httpRequestMessage = new RequestBuilder(HttpMethod.Get, BASE_URL + method)
            .AddParameters(new Dictionary<string, object>
            {
                {"q", location},
                {"limit", "1"},
                {"appid", API_KEY}
            }).Build();

        HttpResponseMessage httpResponseMessage = await RequestSender.DoRequest(httpRequestMessage);
        return !httpResponseMessage.IsSuccessStatusCode 
            ? new LocationRoot()
            : (await httpResponseMessage.Content.ReadAsStringAsync()).ToObject<List<LocationRoot>>().FirstOrDefault();
    }
    
    private static ForecastData MapToForecast(ForecastRoot forecastRoot, LocationRoot locationRoot)
    {
        ForecastData forecastData = new ForecastData();

        List<List<HourWeather>> daysForecast = new List<List<HourWeather>>();
        List<HourWeather> days = new List<HourWeather>();

        int counter = 0;
        foreach (HourWeather listObject in forecastRoot.ListObjects)
        {
            counter++;
            days.Add(listObject);

            if (counter != 8)
            {
                continue;
            }
            
            daysForecast.Add(days);
            days = new List<HourWeather>();
            counter = 0;
        }

        foreach (List<HourWeather> hourWeathers in daysForecast)
        {
            WeatherDayData weatherDayData = new WeatherDayData
            {
                Date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                    .AddSeconds(hourWeathers.FirstOrDefault()?.Dt ?? 0).ToLocalTime().ToString("t"),
                TemperatureMaxC = ((int) (hourWeathers.Max(x=>x.Main.Temp) - 273.15)).ToString(CultureInfo.InvariantCulture),
                TemperatureMaxF = ((int) (hourWeathers.Max(x=>x.Main.Temp) - 273.15 + 32)).ToString(CultureInfo.InvariantCulture),
                TemperatureMinC = ((int) (hourWeathers.Min(x=>x.Main.Temp) - 273.15)).ToString(CultureInfo.InvariantCulture),
                TemperatureMinF = ((int) (hourWeathers.Min(x=>x.Main.Temp) - 273.15  + 32)).ToString(CultureInfo.InvariantCulture),
                AverageTemperatureC = ((int) (hourWeathers.Sum(x => x.Main.Temp - 273.15) / hourWeathers.Count)).ToString(CultureInfo.InvariantCulture),
                AverageTemperatureF = ((int) ((hourWeathers.Sum(x => x.Main.Temp)- 273.15 + 32) / hourWeathers.Count)).ToString(CultureInfo.InvariantCulture),
                WeatherStatus = hourWeathers[4].Weather.FirstOrDefault()?.Main,
                WeatherIconUrl = $"http://openweathermap.org/img/wn/{hourWeathers[4].Weather.FirstOrDefault()?.Icon}@2x.png"
            };
            
            foreach (HourWeather hour in hourWeathers)
            {
                weatherDayData.WeatherDataHours.Add(new WeatherData
                {
                    Date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        .AddSeconds(hour.Dt).ToLocalTime().ToString("t"),
                    Location = $"{locationRoot.Country}, {locationRoot.State}\n{locationRoot.Name}",
                    Pressure = hour.Main.Pressure.ToString(CultureInfo.InvariantCulture),
                    TemperatureC = ((int) hour.Main.Temp).ToString(CultureInfo.InvariantCulture),
                    TemperatureF = ((int) hour.Main.Temp + 32).ToString(CultureInfo.InvariantCulture),
                    WeatherStatus = hour.Weather.FirstOrDefault()?.Main,
                    WeatherIconUrl = $"http://openweathermap.org/img/wn/{hour.Weather.FirstOrDefault()?.Icon}@2x.png",
                    WindDirect = string.Empty,
                    WindSpeed = hour.Wind.Speed.ToString(CultureInfo.InvariantCulture),
                    TemperatureFeelsLikeC = ((int) hour.Main.FeelsLike).ToString(CultureInfo.InvariantCulture),
                    TemperatureFeelsLikeF = ((int) hour.Main.FeelsLike + 32).ToString(CultureInfo.InvariantCulture)
                });
            }
                
            forecastData.WeatherDayDataList.Add(weatherDayData);
        }
        
        return forecastData;
    }
}