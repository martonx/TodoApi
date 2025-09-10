using Data;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class ToDoService(ToDoDbContext db) : IToDoService
{
    public async Task Create(ToDo entity)
    {
        db.ToDos.Add(entity);
        await db.SaveChangesAsync();
    }

    public async Task<List<ToDo>> ListAll()
    {
        return await db.ToDos.ToListAsync();
    }
}
