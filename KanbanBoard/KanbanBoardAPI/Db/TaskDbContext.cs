using Microsoft.EntityFrameworkCore;
using KanbanBoardAPI.Models;
namespace KanbanBoardAPI.Db
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options)
            : base(options)
        {
        }

        public DbSet<KanbanTask> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<KanbanTask>().HasData(
               new KanbanTask
               {
                   Id = Guid.NewGuid(),
                   Title = "[TOL-1] Initiate the project workflow",
                   Description = "Set up the project structure and install dependencies",
                   Status = KanbanTaskStatus.Done,
                   CreatedAt = DateTime.UtcNow.AddDays(-5),
                   UpdatedAt = DateTime.UtcNow.AddDays(-5)
               },
                new KanbanTask
                {
                    Id = Guid.NewGuid(),
                    Title = "[TOL-2] Setup the database schema and tables",
                    Description = "Design and implement database models",
                    Status = KanbanTaskStatus.InProgress,
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new KanbanTask
                {
                    Id = Guid.NewGuid(),
                    Title = "[TOL-3] Configure TypeScript and ESLint",
                    Description = "Configure TypeScript and ESLint for the frontend project",
                    Status = KanbanTaskStatus.Todo,
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new KanbanTask
                {
                    Id = Guid.NewGuid(),
                    Title = "[TOL-4] Configure ALL",
                    Description = "Configure ALL",
                    Status = KanbanTaskStatus.Todo,
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1)
                }
            );
        }
    }
}
