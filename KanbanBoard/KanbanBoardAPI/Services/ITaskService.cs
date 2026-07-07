using KanbanBoardAPI.DTO;

namespace KanbanAPI.Services;

public interface ITaskService
{
    Task<List<TaskDto>> GetAllTasksAsync();
    Task<TaskDto?> GetTaskByIdAsync(Guid id);
    Task<TaskDto> CreateTaskAsync(CreateTaskDto dto);
    Task<TaskDto?> UpdateTaskAsync(Guid id, UpdateTaskDto dto);
    Task<TaskDto?> MoveTaskAsync(Guid id, MoveTaskDto dto);
    Task<bool> DeleteTaskAsync(Guid id);
}