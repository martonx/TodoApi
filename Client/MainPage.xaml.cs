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
            LoadData();
        }

        private void LoadData()
        {
            var httpClient = httpClientFactory.CreateClient();
            var toDos = httpClient.GetFromJsonAsync<List<ToDoDto>>("https://localhost:7241/list").Result;

            toDoCollection.Clear();

            foreach (var toDo in toDos)
                toDoCollection.Add(toDo);
        }

        private void OnDeleteClicked(object? sender, EventArgs e)
        {
            var button = (Button)sender;
            var toDo = (ToDoDto)button.BindingContext;
            //TODO melyik Id-jú elemre kattintottunk
            var httpClient = httpClientFactory.CreateClient();
            var toDos = httpClient.DeleteAsync($"https://localhost:7241/delete/{toDo.Id}").Result;

            LoadData();
        }

        private void OnEditClicked(object? sender, EventArgs e)
        {
            var httpClient = httpClientFactory.CreateClient();
            var toDos = httpClient.GetFromJsonAsync<List<ToDoDto>>("https://localhost:7241/list").Result;

            toDoCollection.Clear();

            foreach (var toDo in toDos)
                toDoCollection.Add(toDo);
        }
    }
}
