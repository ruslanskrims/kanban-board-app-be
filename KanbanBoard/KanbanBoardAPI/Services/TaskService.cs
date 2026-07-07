using AutoMapper;
using KanbanAPI.Services;
using KanbanBoardAPI.Db;
using KanbanBoardAPI.DTO;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoardAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly IMapper _mapper;
        private readonly TaskDbContext _context;

        public TaskService(IMapper mapper, TaskDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<TaskDto>> GetAllTasks()
        {
            var tasks = await _context.Tasks.ToListAsync();
            return _mapper.Map<List<TaskDto>>(tasks);
        }
    }
}