
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;

namespace WeatherWidget
{
    class DatabaseClient
    {
        SQLiteConnection dbConnection = null;
        string path = null;

        public DatabaseClient(string path)
        {
            this.path = path;
        }

        public void Open()
        {
            dbConnection = new SQLiteConnection($"Data Source={path}; Version = 3;");
            dbConnection.Open();
        }

        public void Close()
        {
            dbConnection?.Close();
            dbConnection = null;
        }

        public List<WeatherData> LoadWeatherForecast(string city)
        {
            List<WeatherData> weatherForecast = new List<WeatherData>();

            // получение id города
            string sql = $"SELECT id from CITY WHERE (city='{city}')";
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            long cityId = (long)command.ExecuteScalar();

            // получение данных о погоде
            sql = $"SELECT * FROM WEATHER WHERE (city_id='{cityId}')";
            command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string timestampFrom = (string)reader["timestamp_from"];
                string timestampTo = (string)reader["timestamp_to"];
                double temperature = (double)reader["temperature"];
                double pressure = (double)reader["pressure"];
                double humidity = (double)reader["humidity"];
                double windSpeed = (double)reader["wind_speed"];
                string clouds = (string)reader["clouds"];

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

        public void WriteWeatherForecast(string city, List<WeatherData> weatherForecast)
        {
            // запись города в таблицу city, если его там нет
            string sql = $"INSERT OR IGNORE INTO CITY(city) VALUES ('{city}')";
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();

            // получение id города
            sql = $"SELECT id from CITY WHERE (city='{city}')";
            command = new SQLiteCommand(sql, dbConnection);
            long cityId = (long)command.ExecuteScalar();

            // запись данных о погоде
            foreach (WeatherData weatherData in weatherForecast)
            {
                sql = $"INSERT OR IGNORE INTO WEATHER(city_id, timestamp_from, timestamp_to, temperature, pressure, humidity, wind_speed, clouds)" +
                      $"VALUES ('{cityId}', '{weatherData.TimestampFrom}', '{weatherData.TimestampTo}', '{weatherData.Temperature}', '{weatherData.Pressure}', '{weatherData.Humidity}', '{weatherData.WindSpeed}', '{weatherData.Clouds}')";
                command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();
            }
        }
    }
}
