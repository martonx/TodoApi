using Common;
using Data;

namespace Services;

public interface IToDoService
{
    Task CreateAsync(ToDo entity);
    Task<List<ToDoDto>> ListAllAsync();
    Task DeleteAsync(int id);
    Task UpdateAsync(ToDo model);
}