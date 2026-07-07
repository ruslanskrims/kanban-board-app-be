using KanbanAPI.Services;
using KanbanBoardAPI.Controllers;
using KanbanBoardAPI.DTO;
using KanbanBoardAPI.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace KanbanBoardAPI.Tests.Controllers;

public class TasksControllerTests
{
    private readonly Mock<ITaskService> _mockTaskService;
    private readonly TasksController _controller;

    public TasksControllerTests()
    {
        _mockTaskService = new Mock<ITaskService>();
        _controller = new TasksController(_mockTaskService.Object);
    }

    [Fact]
    public async Task GetTasks_WhenNoTasksExist_ReturnsEmptyArray()
    {
        var emptyTaskList = TestDataHelper.GetEmptyTaskDtoList();
        _mockTaskService
            .Setup(service => service.GetAllTasks())
            .ReturnsAsync(emptyTaskList);

        var result = await _controller.GetTasks();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var tasks = Assert.IsAssignableFrom<IEnumerable<TaskDto>>(okResult.Value);

        Assert.NotNull(tasks);
        Assert.Empty(tasks);
    }

    [Fact]
    public async Task GetTasks_WhenTasksExist_ReturnsNonEmptyArray()
    {
        var taskDtos = TestDataHelper.GetSampleTaskDtos();
        _mockTaskService
            .Setup(service => service.GetAllTasks())
            .ReturnsAsync(taskDtos);

        var result = await _controller.GetTasks();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var tasks = Assert.IsType<List<TaskDto>>(okResult.Value);
        Assert.Equal(3, tasks.Count);
        Assert.Equal(taskDtos.Count, tasks.Count);
    }
}