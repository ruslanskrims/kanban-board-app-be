using AutoMapper;
using AutoMapper.QueryableExtensions;
using KanbanAPI.Services;
using KanbanBoardAPI.Db;
using KanbanBoardAPI.DTO;
using KanbanBoardAPI.Models;
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

        public async Task<List<TaskDto>> GetAllTasksAsync()
        {
            var tasks = await _context.Tasks.ToListAsync();
            return _mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task<TaskDto?> GetTaskByIdAsync(Guid id)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Where(t => t.Id == id)
                .ProjectTo<TaskDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto dto)
        {
            var task = new KanbanTask
            {
                Id = Guid.NewGuid(),
                Title = dto.Title.Trim(),
                Description = dto.Description?.Trim(),
                Status = (KanbanTaskStatus)dto.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return _mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto?> UpdateTaskAsync(Guid id, UpdateTaskDto dto)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return null;

            task.Title = dto.Title.Trim();
            task.Description = dto.Description?.Trim();
            task.Status = (KanbanTaskStatus)dto.Status;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto?> MoveTaskAsync(Guid id, MoveTaskDto dto)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return null;

            task.Status = (KanbanTaskStatus)dto.Status;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<TaskDto>(task);
        }

        public async Task<bool> DeleteTaskAsync(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}