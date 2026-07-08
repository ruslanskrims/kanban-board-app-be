using KanbanAPI.Services;
using KanbanBoardAPI.Controllers;
using KanbanBoardAPI.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using KanbanBoardAPI.DTO;
using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

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
            .Setup(service => service.GetAllTasksAsync())
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
            .Setup(service => service.GetAllTasksAsync())
            .ReturnsAsync(taskDtos);

        var result = await _controller.GetTasks();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var tasks = Assert.IsType<List<TaskDto>>(okResult.Value);
        Assert.Equal(3, tasks.Count);
        Assert.Equal(taskDtos.Count, tasks.Count);
    }

    [Fact]
    public async Task GetTask_WhenExists_ReturnsOk()
    {
        var sample = TestDataHelper.GetSampleTaskDtos().First();
        _mockTaskService
            .Setup(s => s.GetTaskByIdAsync(sample.Id))
            .ReturnsAsync(sample);

        var result = await _controller.GetTask(sample.Id);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsType<TaskDto>(ok.Value);
        Assert.Equal(sample.Id, value.Id);
    }

    [Fact]
    public async Task GetTask_WhenNotFound_ReturnsNotFound()
    {
        var id = Guid.NewGuid();
        _mockTaskService
            .Setup(s => s.GetTaskByIdAsync(id))
            .ReturnsAsync((TaskDto?)null);

        var result = await _controller.GetTask(id);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateTask_ReturnsCreatedAtAction()
    {
        var dto = new CreateTaskDto { Title = "New", Description = "D", Status = 1 };
        var created = new TaskDto { Id = Guid.NewGuid(), Title = dto.Title, Description = dto.Description, Status = dto.Status, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };

        _mockTaskService
            .Setup(s => s.CreateTaskAsync(dto))
            .ReturnsAsync(created);

        var result = await _controller.CreateTask(dto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(TasksController.GetTask), createdResult.ActionName);
        Assert.Equal(created.Id, ((TaskDto)createdResult.Value).Id);
        Assert.Equal(created.Id, createdResult.RouteValues!["id"]);
    }

    [Fact]
    public async Task UpdateTask_WhenExists_ReturnsOk()
    {
        var existing = TestDataHelper.GetSampleTaskDtos().First();
        var dto = new UpdateTaskDto { Title = "Updated", Description = "X", Status = 2 };

        _mockTaskService
            .Setup(s => s.UpdateTaskAsync(existing.Id, dto))
            .ReturnsAsync(new TaskDto { Id = existing.Id, Title = dto.Title, Description = dto.Description, Status = dto.Status, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });

        var result = await _controller.UpdateTask(existing.Id, dto);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsType<TaskDto>(ok.Value);
        Assert.Equal(existing.Id, value.Id);
    }

    [Fact]
    public async Task UpdateTask_WhenNotFound_ReturnsNotFound()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateTaskDto { Title = "X", Description = "Y", Status = 1 };
        _mockTaskService
            .Setup(s => s.UpdateTaskAsync(id, dto))
            .ReturnsAsync((TaskDto?)null);

        var result = await _controller.UpdateTask(id, dto);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task MoveTask_WhenExists_ReturnsOk()
    {
        var existing = TestDataHelper.GetSampleTaskDtos().First();
        var dto = new MoveTaskDto { Status = 3 };

        _mockTaskService
            .Setup(s => s.MoveTaskAsync(existing.Id, dto))
            .ReturnsAsync(new TaskDto { Id = existing.Id, Title = existing.Title, Description = existing.Description, Status = dto.Status, CreatedAt = existing.CreatedAt, UpdatedAt = DateTime.UtcNow });

        var result = await _controller.MoveTask(existing.Id, dto);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsType<TaskDto>(ok.Value);
        Assert.Equal(dto.Status, value.Status);
    }

    [Fact]
    public async Task MoveTask_WhenNotFound_ReturnsNotFound()
    {
        var id = Guid.NewGuid();
        var dto = new MoveTaskDto { Status = 2 };
        _mockTaskService
            .Setup(s => s.MoveTaskAsync(id, dto))
            .ReturnsAsync((TaskDto?)null);

        var result = await _controller.MoveTask(id, dto);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task DeleteTask_WhenDeleted_ReturnsNoContent()
    {
        var id = Guid.NewGuid();
        _mockTaskService
            .Setup(s => s.DeleteTaskAsync(id))
            .ReturnsAsync(true);

        var result = await _controller.DeleteTask(id);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteTask_WhenNotFound_ReturnsNotFound()
    {
        var id = Guid.NewGuid();
        _mockTaskService
            .Setup(s => s.DeleteTaskAsync(id))
            .ReturnsAsync(false);

        var result = await _controller.DeleteTask(id);

        Assert.IsType<NotFoundResult>(result);
    }
}