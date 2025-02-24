using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.Maui.Controls;
using SQLite;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        private const string ApiKey = "d161cde55d236899a22dcf3fc0b3b0f9";
        private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric&lang=pl";

        public MainPage()
        {
            InitializeComponent();
            LoadWeatherHistory();
        }

        private async void LoadWeatherHistory()
        {
            var dbService = new DatabaseService();
            var history = await dbService.GetWeatherHistoryAsync();
            HistoriaListView.ItemsSource = history;
        }

        private async void OnWeatherButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MiastoEntry.Text))
            {
                WynikLabel.Text = "Wpisz nazwę miasta!";
                return;
            }

            string url = string.Format(BaseUrl, MiastoEntry.Text, ApiKey);

            try
            {
                using HttpClient client = new HttpClient();
                string response = await client.GetStringAsync(url);
                JObject json = JObject.Parse(response);

                string miasto = json["name"]?.ToString();
                string temperatura = json["main"]?["temp"]?.ToString();
                string opis = json["weather"]?[0]?["description"]?.ToString();
               
                string wilgotnosc = json["main"]?["humidity"]?.ToString();
                string cisnienie = json["main"]?["pressure"]?.ToString();
                string wiatr = json["wind"]?["speed"]?.ToString();

                WynikLabel.Text = $"Pogoda w {miasto}: {temperatura}°C, {opis}\n" +
                                  $"💧 Wilgotność: {wilgotnosc}%\n" +
                                  $"🌬️ Wiatr: {wiatr} m/s\n" +
                                  $"🌡️ Ciśnienie: {cisnienie} hPa";

                var dbService = new DatabaseService(); // Utworzenie instancji obsługi bazy
                await dbService.SaveWeatherDataAsync(new WeatherData
                {
                    City = miasto,
                    Temperature = temperatura,
                    Description = opis
                });
            }
            catch (Exception ex)
            {
                WynikLabel.Text = "Nie udało się pobrać danych.";
            }
        }
    }
}