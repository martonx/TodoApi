using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;

var builder = WebApplication.CreateBuilder(args);

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
});

app.MapGroup("ToDo").WithTags("ToDo").MapGet("list", async (
    [FromQuery(Name = "isReady")] bool? isReady,
    IToDoService service) =>
{
    return await service.ListAllAsync(isReady);
});

app.MapGroup("ToDo").WithTags("ToDo").MapPost("create", async (ToDo model, IToDoService service) =>
{
    await service.CreateAsync(model);

    return Results.Created();
});

app.MapGroup("ToDo").WithTags("ToDo").MapPut("update", async (ToDo model, IToDoService service) =>
{
    await service.UpdateAsync(model);

    return Results.Ok();
});

app.MapGroup("ToDo").WithTags("ToDo").MapDelete("delete/{id:int}", async (int id, IToDoService service) =>
{
    await service.DeleteAsync(id);

    return Results.Ok();
});

app.Run();

//Ez a tesztekben használhatósághoz kell, hogy publikusan elérhető legyen a Program osztály
public partial class Program;
