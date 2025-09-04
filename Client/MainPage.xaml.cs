using Common;
using System.Net.Http.Json;

namespace Client
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object? sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private void OnWeatherClicked(object? sender, EventArgs e)
        {
            var httpClient = new HttpClient();
            var response = httpClient.GetFromJsonAsync<List<WeatherForecast>>("https://localhost:7241/weatherforecast").Result;

            WeatherInfo.Text = response.First().Summary;
        }
    }
}
