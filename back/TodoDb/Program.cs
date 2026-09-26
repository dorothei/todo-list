using Microsoft.EntityFrameworkCore;
using TodoApp;

var builder = WebApplication.CreateBuilder(args);

// Подключаем SQLite
builder.Services.AddDbContext<TodoDb>(opt => opt.UseSqlite("Data Source=Todo.db"));
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
}

app.UseHttpsRedirection();

// --- Блок авторизации (Раздел 3.1 ТЗ) ---
app.MapPost("/api/auth/register", () => Results.Ok("Заглушка: Регистрация"));
app.MapPost("/api/auth/login", () => Results.Ok("Заглушка: Вход"));
app.MapPost("/api/auth/logout", () => Results.Ok("Заглушка: Выход"));

// --- Блок работы с задачами CRUD (Раздел 3.1 ТЗ) ---
// Обратите внимание: здесь написано db.Tasks, что соответствует свойству из Шага 1
app.MapGet("/api/tasks", async (TodoDb db) => 
    await db.Tasks.ToListAsync());

app.MapPost("/api/tasks", () => Results.Ok("Заглушка: Создание задачи"));
app.MapPut("/api/tasks/{id:int}", (int id) => Results.Ok($"Заглушка: Редактирование {id}"));
app.MapPatch("/api/tasks/{id:int}/complete", (int id) => Results.Ok($"Заглушка: Смена статуса {id}"));
app.MapDelete("/api/tasks/{id:int}", (int id) => Results.Ok($"Заглушка: Удаление {id}"));

// Автоматическое создание БД при старте
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TodoDb>();
    db.Database.EnsureCreated(); 
}

app.Run();
