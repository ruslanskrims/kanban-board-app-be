using KanbanBoardAPI.DTO;
using KanbanBoardAPI.Models;

namespace KanbanBoardAPI.Tests.Helpers;

public static class TestDataHelper
{
    public static List<KanbanTask> GetSampleTasks()
    {
        return new List<KanbanTask>
        {
            new KanbanTask
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Title = "Task 1",
                Description = "Description 1",
                Status = KanbanTaskStatus.Todo,
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                UpdatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new KanbanTask
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Title = "Task 2",
                Description = "Description 2",
                Status = KanbanTaskStatus.InProgress,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new KanbanTask
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Title = "Task 3",
                Description = "Description 3",
                Status = KanbanTaskStatus.Done,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };
    }

    public static List<TaskDto> GetSampleTaskDtos()
    {
        return GetSampleTasks().Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = (int)t.Status,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        }).ToList();
    }

    public static List<KanbanTask> GetEmptyTaskList()
    {
        return new List<KanbanTask>();
    }

    public static List<TaskDto> GetEmptyTaskDtoList()
    {
        return new List<TaskDto>();
    }
}