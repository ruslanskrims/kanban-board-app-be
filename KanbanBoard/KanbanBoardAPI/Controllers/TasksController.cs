using KanbanAPI.Services;
using KanbanBoardAPI.DTO;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoardAPI.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskService taskService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks()
        {
            var tasks = await _taskService.GetAllTasks();

            return Ok(tasks);
        }
    }
}
