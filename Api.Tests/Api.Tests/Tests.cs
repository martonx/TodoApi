using Common;
using System.Net.Http.Json;

namespace Api.Tests
{
    public class Tests
    {
        [ClassDataSource<WebApplicationFactory>(Shared = SharedType.PerTestSession)]
        public required WebApplicationFactory WebApplicationFactory { get; init; }

        private HttpClient client => WebApplicationFactory.CreateClient();

        [BeforeEvery(Test)]
        public static async Task Setup(TestContext context, CancellationToken cancellationToken)
        {
            // Use cancellation token for timeout-aware operations
            //await SomeLongRunningOperation(cancellationToken);
        }

        [Test]
        public async Task TestListing()
        {
            var toDos = await client.GetFromJsonAsync<List<ToDoDto>>("/todo/list");

            await Assert.That(toDos).IsNotEmpty();
        }
    }
}
