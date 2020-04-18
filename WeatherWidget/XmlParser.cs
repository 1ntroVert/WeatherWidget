using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace WeatherWidget
{
    class XmlParser
    {
        public List<WeatherData> ParseWeatherForecast(string xml)
        {
            XmlDocument doc = new XmlDocument();
            // загрузка xml из строки
            doc.LoadXml(xml);
            // получение корневого элемента
            XmlElement root = doc.DocumentElement;

            XmlNode forecastNode = root.GetElementsByTagName("forecast").Item(0);

            List <WeatherData> weatherForecast = new List<WeatherData>();
            foreach (XmlNode node in forecastNode.ChildNodes)
            {
                string timestampFrom = node.Attributes.GetNamedItem("from").InnerText;
                string timestampTo = node.Attributes.GetNamedItem("to").InnerText;
                string temperature = node.SelectSingleNode("temperature").Attributes.GetNamedItem("value").Value;
                string pressure = node.SelectSingleNode("pressure").Attributes.GetNamedItem("value").Value;
                string humidity = node.SelectSingleNode("humidity").Attributes.GetNamedItem("value").Value;
                string windSpeed = node.SelectSingleNode("windSpeed").Attributes.GetNamedItem("mps").Value;
                string clouds = node.SelectSingleNode("clouds").Attributes.GetNamedItem("value").Value;
                WeatherData weatherData = new WeatherData
                {
                    TimestampFrom = DateTime.Parse(timestampFrom),
                    TimestampTo = DateTime.Parse(timestampTo),
                    Temperature = Convert.ToSingle(temperature, CultureInfo.InvariantCulture),
                    Pressure = Convert.ToSingle(pressure, CultureInfo.InvariantCulture),
                    Humidity = Convert.ToSingle(humidity, CultureInfo.InvariantCulture),
                    WindSpeed = Convert.ToSingle(windSpeed, CultureInfo.InvariantCulture),
                    Clouds = clouds
                };
                weatherForecast.Add(weatherData);
            }
            return weatherForecast;
        }
    }
}