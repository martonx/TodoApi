// Here you could define global logic that would affect all tests

// You can use attributes at the assembly level to apply to all tests in the assembly
using Data;
using Microsoft.EntityFrameworkCore;

//[assembly: Retry(3)]
[assembly: System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]

namespace Integration.Tests;

public class GlobalDb
{
    public static ToDoDbContext Db;

    [Before(TestSession)]
    public static async Task SetupDatabase()
    {
        var options = new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        Db = new ToDoDbContext(options);
    }

    [After(TestSession)]
    public static void CleanUp()
    {
        Db.Dispose();
    }
}
