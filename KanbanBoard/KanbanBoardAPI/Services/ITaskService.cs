using KanbanBoardAPI.DTO;

namespace KanbanAPI.Services;

public interface ITaskService
{
    Task<List<TaskDto>> GetAllTasks();
}