using KanbanAPI.Services;
using KanbanBoardAPI.Db;
using KanbanBoardAPI.Mappings;
using KanbanBoardAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlite("Data Source=kanban.db"));

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
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

using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
    db.Database.EnsureCreated();
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