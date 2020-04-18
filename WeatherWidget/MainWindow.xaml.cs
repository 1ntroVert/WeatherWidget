using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;

namespace WeatherWidget
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WeatherApiClient weatherApiClient = new WeatherApiClient();
        DatabaseClient databaseClient = null;

        List<WeatherData> weatherForecast = new List<WeatherData>();

        public MainWindow()
        {
            InitializeComponent();
            cityCombobox.Items.Add("Novosibirsk");
            cityCombobox.Items.Add("Moscow");
            cityCombobox.Items.Add("Sochi");
        }

        #region Converters
        private List<WeatherViewData> convertToWeatherViewData(List<WeatherData> weatherForecast)
        {
            List<WeatherViewData> weatherForecastViewData = new List<WeatherViewData>();
            foreach (WeatherData weatherData in weatherForecast)
            {
                weatherForecastViewData.Add(convertToWeatherViewData(weatherData));
            }
            return weatherForecastViewData;
        }

        private WeatherViewData convertToWeatherViewData(WeatherData weatherData)
        {
            return new WeatherViewData
            {
                DateFrom = weatherData.TimestampFrom.ToShortDateString(),
                TimeFrom = weatherData.TimestampFrom.ToShortTimeString(),
                DateTo = weatherData.TimestampTo.ToShortDateString(),
                TimeTo = weatherData.TimestampTo.ToShortTimeString(),
                Temperature = (weatherData.Temperature - 273.15f).ToString("0.#"),
                Pressure = Convert.ToInt32(weatherData.Pressure * 0.750062f),
                Humidity = weatherData.Humidity,
                WindSpeed = weatherData.WindSpeed,
                Clouds = weatherData.Clouds
            };
        }
        #endregion

        private void networkButton_Click(object sender, RoutedEventArgs e)
        {
            string city = cityCombobox.SelectedItem as string;
            weatherForecast = weatherApiClient.LoadWeatherForecast(city);
            if (weatherForecast != null) weatherDataGrid.ItemsSource = convertToWeatherViewData(weatherForecast);
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (databaseClient == null) openDatabase();
            databaseClient.WriteWeatherForecast(cityCombobox.SelectedItem as string, weatherForecast);
        }

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            if (databaseClient == null) openDatabase();
            weatherForecast = databaseClient.LoadWeatherForecast(cityCombobox.SelectedItem as string);
            if (weatherForecast != null) weatherDataGrid.ItemsSource = convertToWeatherViewData(weatherForecast);
        }

        private void openDatabase()
        {
            OpenFileDialog dbFileDialog = new OpenFileDialog();
            bool? result = dbFileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                databaseClient = new DatabaseClient(dbFileDialog.FileName);
                databaseClient.Open();
            }
        }
    }
}
