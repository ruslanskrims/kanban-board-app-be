using KanbanAPI.Services;
using KanbanBoardAPI.Db;
using KanbanBoardAPI.DTO;
using KanbanBoardAPI.Models;
using KanbanBoardAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
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

builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<KanbanTask, TaskDto>()
        .ForMember(dest => dest.Id,
            opt => opt.MapFrom(src => src.Id));
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowVueApp");
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();