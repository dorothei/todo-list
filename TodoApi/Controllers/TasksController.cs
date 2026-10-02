using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Db;

namespace TodoApi.Controllers;

public record TaskCreateUpdateDto(
    [property: Required(ErrorMessage = "Название задачи обязательно.")]
    [property: MinLength(1), MaxLength(200)]
    string Title,

    [property: MaxLength(2000)]
    string? Description,

    bool Completed);

public record TaskResponseDto(
    int Id,
    string Title,
    string? Description,
    bool Completed,
    DateTime UpdatedAt);

public record DeleteResponseDto(string Message);

[ApiController]
[Route("api/tasks")]
[Authorize]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public class TasksController : ControllerBase
{
    private readonly TodoDb _db;

    public TasksController(TodoDb db)
    {
        _db = db;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TaskResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasks()
    {
        var userId = GetUserId();
        if (userId == null) return UnauthorizedProblem();

        var tasks = await _db.Tasks
            .Where(t => t.UserId == userId.Value)
            .Select(t => new TaskResponseDto(
                t.Id, t.Title, t.Description, t.Completed, t.UpdatedAt))
            .ToListAsync();

        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TaskResponseDto>> GetTask(int id)
    {
        var userId = GetUserId();
        if (userId == null) return UnauthorizedProblem();

        var task = await _db.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId.Value);

        if (task == null) return NotFoundProblem("Задача не найдена.");

        return Ok(new TaskResponseDto(
            task.Id, task.Title, task.Description, task.Completed, task.UpdatedAt));
    }

    [HttpPost]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateTask([FromBody] TaskCreateUpdateDto dto)
    {
        var userId = GetUserId();
        if (userId == null) return UnauthorizedProblem();

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
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskCreateUpdateDto dto)
    {
        var userId = GetUserId();
        if (userId == null) return UnauthorizedProblem();

        var task = await _db.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId.Value);

        if (task == null) return NotFoundProblem("Задача не найдена.");

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Completed = dto.Completed;
        task.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok(new TaskResponseDto(
            task.Id, task.Title, task.Description, task.Completed, task.UpdatedAt));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(DeleteResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var userId = GetUserId();
        if (userId == null) return UnauthorizedProblem();

        var task = await _db.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId.Value);

        if (task == null) return NotFoundProblem("Задача не найдена.");

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();

        return Ok(new DeleteResponseDto($"Задача с ID {id} успешно удалена."));
    }

    [HttpPatch("{id:int}/complete")]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ToggleComplete(int id)
    {
        var userId = GetUserId();
        if (userId == null) return UnauthorizedProblem();

        var task = await _db.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId.Value);

        if (task == null) return NotFoundProblem("Задача не найдена.");

        task.Completed = !task.Completed;
        task.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok(new TaskResponseDto(
            task.Id, task.Title, task.Description, task.Completed, task.UpdatedAt));
    }

    private int? GetUserId()
    {
        var userIdClaim =
            User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

        return int.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    // --- Хелперы для единообразных ошибок ---

    private ObjectResult UnauthorizedProblem() =>
        Unauthorized(new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized",
            Detail = "Пользователь не авторизован."
        });

    private ObjectResult NotFoundProblem(string detail) =>
        NotFound(new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Not Found",
            Detail = detail
        });
}