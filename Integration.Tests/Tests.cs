using Data;
using Integration.Tests.Data;
using Microsoft.EntityFrameworkCore;

using Services;

namespace Integration.Tests;

public class Tests
{
    [Before(Test)]
    public async Task SetupTest()
    {
        var allTodos = await GlobalDb.Db.ToDos.ToListAsync();
        GlobalDb.Db.ToDos.RemoveRange(allTodos);

        GlobalDb.Db.ToDos.Add(new ToDo
        {
            Title = "Initial ToDo",
            Description = "This is a pre-existing ToDo item",
            IsReady = false,
            Created = DateTime.Now
        });
        await GlobalDb.Db.SaveChangesAsync();
    }

    [Test]
    public async Task BasicService()
    {
        var service = new ToDoService(GlobalDb.Db);
        var todos = await service.ListAllAsync(null);

        await Assert.That(todos.Count).IsEqualTo(1);
    }

    [Test]
    [Arguments(1, 2, 3)]
    [Arguments(2, 3, 5)]
    public async Task DataDrivenArguments(int a, int b, int c)
    {
        Console.WriteLine("This one can accept arguments from an attribute");

        var result = a + b;

        await Assert.That(result).IsEqualTo(c);
    }

    [Test]
    [MethodDataSource(nameof(DataSource))]
    public async Task MethodDataSource(int a, int b, int c)
    {
        Console.WriteLine("This one can accept arguments from a method");

        var result = a + b;

        await Assert.That(result).IsEqualTo(c);
    }

    [Test]
    [ClassDataSource<DataClass>]
    [ClassDataSource<DataClass>(Shared = SharedType.PerClass)]
    [ClassDataSource<DataClass>(Shared = SharedType.PerAssembly)]
    [ClassDataSource<DataClass>(Shared = SharedType.PerTestSession)]
    public void ClassDataSource(DataClass dataClass)
    {
        Console.WriteLine("This test can accept a class, which can also be pre-initialised before being injected in");

        Console.WriteLine("These can also be shared among other tests, or new'd up each time, by using the `Shared` property on the attribute");
    }

    [Test]
    [DataGenerator]
    public async Task CustomDataGenerator(int a, int b, int c)
    {
        Console.WriteLine("You can even define your own custom data generators");

        var result = a + b;

        await Assert.That(result).IsEqualTo(c);
    }

    public static IEnumerable<(int a, int b, int c)> DataSource()
    {
        yield return (1, 1, 2);
        yield return (2, 1, 3);
        yield return (3, 1, 4);
    }
}
