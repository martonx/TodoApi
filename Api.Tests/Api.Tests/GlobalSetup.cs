// Here you could define global logic that would affect all tests

// You can use attributes at the assembly level to apply to all tests in the assembly
using Api.Tests;

using Data;

using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

[assembly: System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]

public class GlobalApi
{
    [ClassDataSource<WebApplicationFactory>(Shared = SharedType.PerTestSession)]
    public static WebApplicationFactory WebApplicationFactory { get; set; }

    public static HttpClient Client;

    [Before(TestSession)]
    public static async Task SetupApiClient()
    {
        Client = WebApplicationFactory.CreateClient();
        var response = await Client.PostAsJsonAsync("/account/login",
            new LoginRequest { Email = "lajosmarton@hotmail.com", Password = "Password1." });
        var responseString = await response.Content.ReadAsStringAsync();
        var jwtResponse = JsonSerializer.Deserialize<AccessTokenResponse>(responseString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse.AccessToken);
    }

    [After(TestSession)]
    public static void CleanUp()
    {
        Client.Dispose();
    }
}
