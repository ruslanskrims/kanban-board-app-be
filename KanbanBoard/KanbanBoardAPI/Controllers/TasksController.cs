using KanbanAPI.Services;
using KanbanBoardAPI.DTO;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoardAPI.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [Produces("application/json")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTask(Guid id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            return task == null ? NotFound() : Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskDto>> CreateTask(CreateTaskDto dto)
        {
            var task = await _taskService.CreateTaskAsync(dto);
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TaskDto>> UpdateTask(Guid id, UpdateTaskDto dto)
        {
            var task = await _taskService.UpdateTaskAsync(id, dto);
            return task == null ? NotFound() : Ok(task);
        }

        [HttpPatch("{id}/move")]
        public async Task<ActionResult<TaskDto>> MoveTask(Guid id, MoveTaskDto dto)
        {
            var task = await _taskService.MoveTaskAsync(id, dto);
            return task == null ? NotFound() : Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(Guid id)
        {
            var deleted = await _taskService.DeleteTaskAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
