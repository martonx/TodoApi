using Common;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity.Data;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Api.Tests;

public class Tests
{
    [ClassDataSource<WebApplicationFactory>(Shared = SharedType.PerTestSession)]
    public required WebApplicationFactory WebApplicationFactory { get; init; }

    public HttpClient client;

    [Before(Test)]
    public async Task Setup(TestContext context, CancellationToken cancellationToken)
    {
        client = WebApplicationFactory.CreateClient();
        var response = await client.PostAsJsonAsync("/account/login",
            new LoginRequest { Email = "lajosmarton@hotmail.com", Password = "Lali1978*" });
        var responseString = await response.Content.ReadAsStringAsync();
        var jwtResponse = JsonSerializer.Deserialize<AccessTokenResponse>(responseString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse.AccessToken);
    }

    [Test]
    public async Task TestListing()
    {
        var toDos = await client.GetFromJsonAsync<List<ToDoDto>>("/todo/list");

        await Assert.That(toDos).IsNotEmpty();
    }
}
