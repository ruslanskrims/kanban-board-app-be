using KanbanAPI.Services;
using KanbanBoardAPI.Db;
using KanbanBoardAPI.DTO;
using KanbanBoardAPI.Models;
using KanbanBoardAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlite("Data Source=kanban.db"));

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<KanbanTask, TaskDto>()
        .ForMember(dest => dest.Id,
            opt => opt.MapFrom(src => src.Id));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .WithMethods
                  ("GET", "POST", "PUT", "DELETE")
                  .WithHeaders("Content-Type", "Authorization", "Accept");
        });
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TaskDbContext>();

    try
    {
        dbContext.Database.EnsureCreated();
        if (!dbContext.Tasks.Any())
        {

            var sampleTasks = new List<KanbanTask>
            {
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
            };

            dbContext.Tasks.AddRange(sampleTasks);
            await dbContext.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        throw; 
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowVueApp");
app.UseAuthorization();
app.MapControllers();

app.Run();