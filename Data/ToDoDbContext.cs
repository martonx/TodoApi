using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Data;

public class ToDoDbContext(DbContextOptions<ToDoDbContext> options) : IdentityDbContext(options)
{
    public DbSet<ToDo> ToDos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseAsyncSeeding(async (context, _, cancellationToken) =>
        {
            await InsertRolesAsync(context, cancellationToken);
        });
    }

    private async Task InsertRolesAsync(DbContext context, CancellationToken cancellationToken)
    {
        var roles = context.Set<IdentityRole>();
        if (!roles.Any(role => role.Name == "Admin"))
        {
            roles.Add(new IdentityRole { Name = "Admin" });
            roles.Add(new IdentityRole { Name = "User" });

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
