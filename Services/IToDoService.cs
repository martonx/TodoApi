using Data;

namespace Services;

public interface IToDoService
{
    Task CreateAsync(ToDo entity);
    Task<List<ToDo>> ListAllAsync();
}