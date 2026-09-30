using Microsoft.EntityFrameworkCore;
using TodoApi.Db;

var builder = WebApplication.CreateBuilder(args);

// Подключаем базу данных SQLite
builder.Services.AddDbContext<TodoDb>(opt => opt.UseSqlite("Data Source=Todo.db"));

builder.Services.AddControllers();

// Настройка CORS для фронтенда
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Настройка Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Страница документации будет доступна на http://localhost:5177/swagger/
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Автоматическое создание файла базы данных при первом старте
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TodoDb>();
    db.Database.EnsureCreated(); 
}

app.Run();
