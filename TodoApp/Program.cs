using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<TodoDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

app.UseMiddleware<LoggingMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run("http://localhost:5000");
