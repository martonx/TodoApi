﻿using Common;
using Data;

namespace Services;

public interface IToDoService
{
    Task CreateAsync(ToDo entity);
    Task<ToDoDto> GetAsync(int id);
    Task<List<ToDoDto>> ListAllAsync(bool? isReady);
    Task DeleteAsync(int id);
    Task UpdateAsync(ToDo model);
}