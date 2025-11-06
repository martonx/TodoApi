using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;

//Ez a sor felel az alap web szerver létrehozásáért.
//Builder pattern-t használ, ahol a konfigurációs beállításokat a builder objektumon keresztül adjuk meg.
var builder = WebApplication.CreateBuilder(args);

//Elkezdjük a buildert felparaméterezni a szükséges szolgáltatásokkal és konfigurációkkal.

builder.Services.AddOpenApiDocument(config =>
{
    config.AddSecurity("JWT", new NSwag.OpenApiSecurityScheme
    {
        Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = NSwag.OpenApiSecurityApiKeyLocation.Header,
        Description = "Type 'Bearer {your JWT token}' into the field below."
    });

    config.OperationProcessors.Add(new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

builder.Services.AddAuthorizationBuilder()
  .AddPolicy("admin", policy => policy.RequireRole("Admin"))
  .AddPolicy("user", policy => policy.RequireRole("User"));

//Ez a tesztekben használhatósághoz kell, hogy a teszt framework elérje az endpointokat
builder.Services.AddEndpointsApiExplorer();

// Configure the Database connection
var connectionString = builder.Configuration.GetConnectionString("ToDoDbContext");
builder.Services.AddDbContext<ToDoDbContext>(options =>
  options.UseSqlServer(connectionString));

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ToDoDbContext>();

// Configure the DI container
builder.Services.AddTransient<IToDoService, ToDoService>();

builder.Services.AddAuthorization();

var allowSpecificOrigins = "_allowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(allowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                      });
});

//A fentebb megadott konfigurációk alapján létrehozzuk a web alkalmazás objektumot.
var app = builder.Build();

app.UseCors(allowSpecificOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.MapGroup("Account").WithTags("Account").MapIdentityApi<IdentityUser>();

app.MapGroup("ToDo").WithTags("ToDo").MapGet("get/{id:int}", async (int id, IToDoService service) =>
{
    return await service.GetAsync(id);
}).RequireAuthorization();

app.MapGroup("ToDo").WithTags("ToDo").MapGet("list", async (
    [FromQuery(Name = "isReady")] bool? isReady,
    IToDoService service) =>
{
    return await service.ListAllAsync(isReady);
}).RequireAuthorization();

app.MapGroup("ToDo").WithTags("ToDo").MapPost("create", async (ToDo model, IToDoService service) =>
{
    await service.CreateAsync(model);

    return Results.Created();
}).RequireAuthorization("admin");

app.MapGroup("ToDo").WithTags("ToDo").MapPut("update", async (ToDo model, IToDoService service) =>
{
    await service.UpdateAsync(model);

    return Results.Ok();
}).RequireAuthorization();

app.MapGroup("ToDo").WithTags("ToDo").MapDelete("delete/{id:int}", async (int id, IToDoService service) =>
{
    await service.DeleteAsync(id);

    return Results.Ok();
}).RequireAuthorization();

//Auto migration
using var serviceScope = (app as IApplicationBuilder).ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
using var dbContext = serviceScope.ServiceProvider.GetService<ToDoDbContext>();
var migrations = await dbContext.Database.GetPendingMigrationsAsync();
if (migrations.Any())
{
    await dbContext.Database.MigrateAsync();
}

var roles = dbContext.Set<IdentityRole>();
if (!await roles.AnyAsync(role => role.Name == "Admin"))
{
    roles.Add(new IdentityRole { Name = "SuperAdmin" });
    roles.Add(new IdentityRole { Name = "Admin" });
    roles.Add(new IdentityRole { Name = "User" });

    await dbContext.SaveChangesAsync();
}

app.Run();

//Ez a tesztekben használhatósághoz kell, hogy publikusan elérhető legyen a Program osztály
public partial class Program;
