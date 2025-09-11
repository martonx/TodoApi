using Data;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class ToDoService(ToDoDbContext db) : IToDoService
{
    public async Task CreateAsync(ToDo entity)
    {
        entity.Created = DateTime.Now;
        db.ToDos.Add(entity);
        await db.SaveChangesAsync();
    }

    public async Task<List<ToDo>> ListAllAsync()
    {
        return await db.ToDos.ToListAsync();
    }
}
