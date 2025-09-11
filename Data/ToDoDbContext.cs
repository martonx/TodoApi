using Microsoft.EntityFrameworkCore;

namespace Data;

public class ToDoDbContext(DbContextOptions<ToDoDbContext> options) : DbContext(options)
{
    public DbSet<ToDo> ToDos { get; set; }
}
