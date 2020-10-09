using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WeatherWidget
{
    class JsonParser : IDataParser
    {
        public List<WeatherData> ParseWeatherForecast(string response)
        {
            var root = JsonConvert.DeserializeObject<Root>(response);
            return toWeatherDataList(root);
        }

        private List<WeatherData> toWeatherDataList(Root root)
        {
            return root.list.Select(list => toWeatherData(list)).ToList();
        }

        private WeatherData toWeatherData(List list)
        {
            return new WeatherData()
            {
                TimestampFrom = DateTime.Parse(list.dt_txt),
                TimestampTo = DateTime.Parse(list.dt_txt),
                Temperature = Convert.ToSingle(list.main.temp, CultureInfo.InvariantCulture),
                Pressure = Convert.ToSingle(list.main.pressure, CultureInfo.InvariantCulture),
                Humidity = Convert.ToSingle(list.main.humidity, CultureInfo.InvariantCulture),
                WindSpeed = Convert.ToSingle(list.wind.speed, CultureInfo.InvariantCulture),
                Clouds = list.clouds.all.ToString()
            };
        }
    }
}
