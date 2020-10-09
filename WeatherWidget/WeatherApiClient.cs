using System.Collections.Generic;

using RestSharp;

namespace WeatherWidget
{
    interface IWeatherApiClient
    {
        List<WeatherData> LoadWeatherForecast(string city = "Novosibirsk");
    }

    class WeatherApiClient<T> : IWeatherApiClient where T : IClientMode, new()
    {
        const string API_KEY = "2d4486f0c9f74101b856053893d94400";
        const string URL = "https://api.openweathermap.org/data/2.5/forecast";
        const string PARAMS = "?q={0}&mode={1}&appid={2}";

        // режим работы клиента
        T mode = new T();

        // REST-клиент
        readonly RestClient client = new RestClient(URL) { Timeout = -1 };

        public List<WeatherData> LoadWeatherForecast(string city)
        {
            string requestUri = string.Format(PARAMS, city, mode.Mode, API_KEY);
            var request = new RestRequest(requestUri, Method.GET) { RequestFormat = mode.DataFormat };
            IRestResponse response = client.Execute(request);

            List<WeatherData> weatherForecast = mode.DataParser.ParseWeatherForecast(response.Content);
            return weatherForecast;
        }
    }
}