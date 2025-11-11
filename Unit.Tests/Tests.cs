using Services;
using Data;

namespace Unit.Tests;

public class Tests
{
    [Test]
    public async Task TodoServiceTest1()
    {
        //SETUP
        var service = new ToDoService(null);
        var testEntity = new ToDo
        {
            Id = 1,
            Title = "Test",
            Created = DateTime.Now,
            Deadline = DateTime.Now.AddDays(1),
            Description = "Description",
            IsReady = false
        };

        //ACT
        var dto = service.CreateDtoFrom(testEntity);

        //ASSERT
        await Assert.That(dto.Id).IsEqualTo(testEntity.Id);
        await Assert.That(dto.Title).IsEqualTo(testEntity.Title);
        await Assert.That(dto.Created).IsEqualTo(testEntity.Created);
        await Assert.That(dto.Deadline).IsEqualTo(testEntity.Deadline);
        await Assert.That(dto.Description).IsEqualTo(testEntity.Description);
        await Assert.That(dto.IsReady).IsEqualTo(testEntity.IsReady);
    }
}
