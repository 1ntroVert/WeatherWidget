using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WeatherWidget
{
    class WeatherApiClient
    {
        const string API_KEY = "2d4486f0c9f74101b856053893d94400";
        const string URL = "https://api.openweathermap.org/data/2.5/forecast";
        const string PARAMS = "?q={0}&mode=xml&appid={1}";

        // создание HTTP-клиента
        HttpClient client = new HttpClient();
        // создание XML-парсера
        XmlParser xmlParser = new XmlParser();

        public WeatherApiClient()
        {
            // установка базового адреса интернет-ресурса
            client.BaseAddress = new Uri(URL);
            // установка заголовков запроса
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
        }

        public List<WeatherData> LoadWeatherForecast(string city = "Novosibirsk")
        {
            List<WeatherData> weatherForecast = null;
            string requestUri = string.Format(PARAMS, city, API_KEY);
            HttpResponseMessage response = client.GetAsync(requestUri).Result;
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                weatherForecast = xmlParser.ParseWeatherForecast(responseString);
            }
            return weatherForecast;
        }
    }
}