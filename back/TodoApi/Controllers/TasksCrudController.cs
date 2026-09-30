using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Db; // Правильный namespace вашей БД

namespace TodoApi.Controllers;

// DTO для обмена данными с фронтендом (Защита от перезаписи UserId/CreatedAt руками клиента)
public record TaskCreateUpdateDto(string Title, string? Description, bool Completed);
public record TaskResponseDto(int Id, string Title, string? Description, bool Completed, DateTime UpdatedAt);

[ApiController]
[Route("api/tasks")]
public class TasksCrudController : ControllerBase
{
    private readonly TodoDb _db;

    public TasksCrudController(TodoDb db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasks()
    {
        var tasks = await _db.Tasks
            .Select(t => new TaskResponseDto(t.Id, t.Title, t.Description, t.Completed, t.UpdatedAt))
            .ToListAsync();
            
        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponseDto>> GetTask(int id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task == null) return NotFound("Задача не найдена.");

        return Ok(new TaskResponseDto(task.Id, task.Title, task.Description, task.Completed, task.UpdatedAt));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskCreateUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            return BadRequest("Название задачи обязательно.");
        }

        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Completed = dto.Completed,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            UserId = 1 // Дефолтный ID из OnModelCreating
        };

        // Заглушка: вяжем к первому юзеру, пока Бэкендер №1 пилит auth
        var firstUser = await _db.Users.FirstOrDefaultAsync();
        if (firstUser != null)
        {
            task.UserId = firstUser.Id;
        }

        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();

        var response = new TaskResponseDto(task.Id, task.Title, task.Description, task.Completed, task.UpdatedAt);
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskCreateUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            return BadRequest("Название задачи не может быть пустым.");
        }

        var task = await _db.Tasks.FindAsync(id);
        if (task == null) return NotFound("Задача не найдена.");

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Completed = dto.Completed;
        task.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        
        return Ok(new TaskResponseDto(task.Id, task.Title, task.Description, task.Completed, task.UpdatedAt));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task == null) return NotFound("Задача не найдена.");

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();

        return Ok(new { message = $"Задача с ID {id} успешно удалена." });
    }
}
