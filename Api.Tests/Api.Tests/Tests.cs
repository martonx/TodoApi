using Common;
using System.Net.Http.Json;

namespace Api.Tests;

public class Tests
{
    [Test]
    public async Task TestListing()
    {
        var toDos = await GlobalApi.Client.GetFromJsonAsync<List<ToDoDto>>("/todo/list");

        await Assert.That(toDos).IsNotEmpty();
    }
}
