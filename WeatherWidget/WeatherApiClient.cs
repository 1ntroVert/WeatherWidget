using System.Collections.Generic;

using RestSharp;

namespace WeatherWidget
{
    class WeatherApiClient
    {
        const string API_KEY = "2d4486f0c9f74101b856053893d94400";
        const string URL = "https://api.openweathermap.org/data/2.5/forecast";
        const string PARAMS = "?q={0}&mode=xml&appid={1}";

        // создание XML-парсера
        XmlParser xmlParser = new XmlParser();

        // REST-клиент
        readonly RestClient client = new RestClient(URL) { Timeout = -1 };

        public List<WeatherData> LoadWeatherForecast(string city = "Novosibirsk")
        {
            string requestUri = string.Format(PARAMS, city, API_KEY);
            var request = new RestRequest(requestUri, Method.GET) { RequestFormat = DataFormat.Xml };
            IRestResponse response = client.Execute(request);

            List<WeatherData> weatherForecast = xmlParser.ParseWeatherForecast(response.Content);
            return weatherForecast;
        }
    }
}