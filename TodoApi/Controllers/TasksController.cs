using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Db;

namespace TodoApi.Controllers;

public record TaskCreateUpdateDto(
    string Title,
    string? Description,
    bool Completed
);

public record TaskResponseDto(
    int Id,
    string Title,
    string? Description,
    bool Completed,
    DateTime UpdatedAt
);

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly TodoDb _db;

    public TasksController(TodoDb db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasks()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var tasks = await _db.Tasks
            .Where(t => t.UserId == userId.Value)
            .Select(t => new TaskResponseDto(
                t.Id, t.Title, t.Description, t.Completed, t.UpdatedAt))
            .ToListAsync();

        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponseDto>> GetTask(int id)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var task = await _db.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId.Value);

        if (task == null) return NotFound("Задача не найдена.");

        return Ok(new TaskResponseDto(
            task.Id, task.Title, task.Description, task.Completed, task.UpdatedAt));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskCreateUpdateDto dto)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest("Название задачи обязательно.");

        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Completed = dto.Completed,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            UserId = userId.Value
        };

        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTask),
            new { id = task.Id },
            new TaskResponseDto(
                task.Id, task.Title, task.Description, task.Completed, task.UpdatedAt));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskCreateUpdateDto dto)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest("Название задачи не может быть пустым.");

        var task = await _db.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId.Value);

        if (task == null) return NotFound("Задача не найдена.");

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Completed = dto.Completed;
        task.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok(new TaskResponseDto(
            task.Id, task.Title, task.Description, task.Completed, task.UpdatedAt));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var task = await _db.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId.Value);

        if (task == null) return NotFound("Задача не найдена.");

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();

        return Ok(new { message = $"Задача с ID {id} успешно удалена." });
    }

    [HttpPatch("{id:int}/complete")]
    public async Task<IActionResult> ToggleComplete(int id)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var task = await _db.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId.Value);

        if (task == null) return NotFound("Задача не найдена.");

        task.Completed = !task.Completed;
        task.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok(new TaskResponseDto(
            task.Id, task.Title, task.Description, task.Completed, task.UpdatedAt));
    }

    private int? GetUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}