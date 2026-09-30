using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Db;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TasksViewController : ControllerBase
{
    private readonly TodoDb _db;

    public TasksViewController(TodoDb db)
    {
        _db = db;
    }

    // PATCH: api/tasks/{id}/complete
    [HttpPatch("{id}/complete")]
    public async Task<IActionResult> ToggleComplete(int id)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var task = await _db.Tasks
            .FirstOrDefaultAsync(t =>
                t.Id == id &&
                t.UserId == userId);

        if (task == null)
        {
            return NotFound("Задача не найдена.");
        }

        task.Completed = !task.Completed;
        task.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok(task);
    }
}