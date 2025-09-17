using Common;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace Client
{
    public partial class MainPage : ContentPage
    {
        private readonly IHttpClientFactory httpClientFactory;
        private ObservableCollection<ToDoDto> toDoCollection = [];
        int count = 0;

        public MainPage(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
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
            var httpClient = httpClientFactory.CreateClient();
            var toDos = httpClient.GetFromJsonAsync<List<ToDoDto>>("https://localhost:7241/list").Result;

            foreach (var toDo in toDos)
                toDoCollection.Add(toDo);
        }
    }
}
