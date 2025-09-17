using Common;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace Client
{
    public partial class MainPage : ContentPage
    {
        private readonly IHttpClientFactory httpClientFactory;
        private ObservableCollection<ToDoDto> toDoCollection = [];

        public MainPage(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            InitializeComponent();

            ToDosView.ItemsSource = toDoCollection;
        }

        private void OnWeatherClicked(object? sender, EventArgs e)
        {
            var httpClient = httpClientFactory.CreateClient();
            var toDos = httpClient.GetFromJsonAsync<List<ToDoDto>>("https://localhost:7241/list").Result;

            toDoCollection.Clear();

            foreach (var toDo in toDos)
                toDoCollection.Add(toDo);
        }
    }
}
