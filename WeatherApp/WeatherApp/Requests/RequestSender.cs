using System.Net.Http;
using System.Threading.Tasks;

namespace WeatherApp.Requests
{
    public static class RequestSender
    {
        private static readonly HttpClient HttpClient = new ();

        public static async Task<HttpResponseMessage> DoRequest(HttpRequestMessage httpRequestMessage)
        {
            return await HttpClient.SendAsync(httpRequestMessage);
        }
    }
}