using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace WeatherApp.Requests
{
    public class RequestBuilder
    {
        private readonly HttpRequestMessage _httpRequestMessage = new ();
        private Dictionary<string, object> _headers = new ();
        private Dictionary<string, object> _parameters = new ();
        private string _requestUrl;

        public RequestBuilder(HttpMethod httpMethod, string requestUrl)
        {
            _httpRequestMessage.Method = httpMethod;
            _requestUrl = requestUrl;
        }

        public RequestBuilder AddHeaders(Dictionary<string, object> headers)
        {
            _headers = headers;
            return this;
        }
    
        public RequestBuilder AddParameters(Dictionary<string, object> parameters)
        {
            _parameters = parameters;
            return this;
        }

        public HttpRequestMessage Build()
        {
            foreach (var header in _headers)
            {
                _httpRequestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToString());
            }

            _requestUrl += $"?{string.Join("&", _parameters.Select(x => $"{x.Key}={x.Value}"))}";
            _httpRequestMessage.RequestUri = new Uri(_requestUrl);
            return _httpRequestMessage;
        }
    }
}