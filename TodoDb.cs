using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TodoApp;

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
    public string Title { get; set; } = null!; // Обязательное поле
    
    public string? Description { get; set; }
    public bool Completed { get; set; } = false;
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
    public DbSet<TaskItem> Tasks => Set<TaskItem>(); // Таблица называется Тasks!
}
