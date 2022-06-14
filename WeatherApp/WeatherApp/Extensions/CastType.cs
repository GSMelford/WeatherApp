using Newtonsoft.Json;

namespace WeatherApp.Extensions
{
    public static class CastType
    {
        public static T ToObject<T>(this string content) where T: new()
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(content) ?? new T();
            }
            catch
            {
                return new T();
            }
        }

        public static int ToInt(this string value)
        {
            return int.TryParse(value, out int result) ? result : default;
        }
        
        public static double ToDouble(this string value)
        {
            return double.TryParse(value, out double result) ? result : default;
        }
    }
}