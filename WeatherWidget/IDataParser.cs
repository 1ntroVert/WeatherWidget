using System.Collections.Generic;

namespace WeatherWidget
{
    interface IDataParser
    {
        List<WeatherData> ParseWeatherForecast(string response);
    }
}
