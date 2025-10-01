using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure the Database connection
var connectionString = builder.Configuration.GetConnectionString("ToDoDbContext");
builder.Services.AddDbContext<ToDoDbContext>(options =>
  options.UseSqlServer(connectionString));

// Configure the DI container
builder.Services.AddTransient<IToDoService, ToDoService>();

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
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("get/{id:int}", async (int id, IToDoService service) =>
{
    return await service.GetAsync(id);
});

app.MapGet("list", async (
    [FromQuery(Name = "isReady")] bool? isReady,
    IToDoService service) =>
{
    return await service.ListAllAsync(isReady);
});

app.MapPost("create", async (ToDo model, IToDoService service) =>
{
    await service.CreateAsync(model);

    return Results.Created();
});

app.MapPut("update", async (ToDo model, IToDoService service) =>
{
    await service.UpdateAsync(model);

    return Results.Ok();
});

app.MapDelete("delete/{id:int}", async (int id, IToDoService service) =>
{
    await service.DeleteAsync(id);

    return Results.Ok();
});

app.Run();
