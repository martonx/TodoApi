using Data;

namespace Services;

public interface IToDoService
{
    Task CreateAsync(ToDo entity);
    Task<List<ToDo>> ListAllAsync();
    Task DeleteAsync(int id);
    Task UpdateAsync(ToDo model);
}