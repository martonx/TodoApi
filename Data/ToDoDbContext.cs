using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class ToDoDbContext(DbContextOptions<ToDoDbContext> options) : IdentityDbContext(options)
{
    public DbSet<ToDo> ToDos { get; set; }
}
