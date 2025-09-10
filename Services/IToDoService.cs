using Data;

namespace Services;

public interface IToDoService
{
    Task Create(ToDo entity);
    Task<List<ToDo>> ListAll();
}