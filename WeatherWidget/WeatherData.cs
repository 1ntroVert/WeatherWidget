using System;

namespace WeatherWidget
{
    class WeatherData
    {
        public DateTime TimestampFrom { get; set; }
        public DateTime TimestampTo { get; set; }
        public float Temperature { get; set; }
        public float Pressure { get; set; }
        public float Humidity { get; set; }
        public float WindSpeed { get; set; }
        public string Clouds { get; set; }
    }
}

