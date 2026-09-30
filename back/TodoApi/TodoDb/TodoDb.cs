using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Db;

// Модель Пользователя (Раздел 2.1 ТЗ)
public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    
    [Required]
    public string Email { get; set; } = null!;
    
    [Required]
    public string PasswordHash { get; set; } = null!;
    
    public List<TaskItem> Tasks { get; set; } = new();
}

// Модель Задачи (Раздел 2.2 ТЗ)
public class TaskItem
{
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; } = null!; 
    
    public string? Description { get; set; }
    public bool Completed { get; set; } = false; // <--- Название из вашей БД
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Связь с пользователем
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

// Контекст Базы Данных
public class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>(); 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Инициализируем тестового пользователя, чтобы код Бэкендера №1 не ломал создание задач
        modelBuilder.Entity<User>().HasData(
            new User 
            { 
                Id = 1, 
                Name = "Тестовый Пользователь", 
                Email = "test@todo.com", 
                PasswordHash = "AQAAAAIAAYagAAAAE..." // Временный фейковый хэш
            }
        );
    }
}
