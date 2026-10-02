using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Db;

namespace TodoApi.Controllers;

public record RegisterRequest(
    [property: Required, MinLength(2), MaxLength(100)] string Name,
    [property: Required, EmailAddress, MaxLength(256)] string Email,
    [property: Required, MinLength(8), MaxLength(128)] string Password);

public record LoginRequest(
    [property: Required, EmailAddress, MaxLength(256)] string Email,
    [property: Required, MaxLength(128)] string Password);

public record RegisterSuccessResponseDto(
    string Message,
    int UserId,
    string Email);

public record LoginSuccessResponseDto(
    string Token,
    int UserId,
    string Email);

[ApiController]
[Route("api/auth")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public class AuthController : ControllerBase
{
    private const int DefaultTokenLifetimeHours = 24;

    private readonly TodoDb _db;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        TodoDb db,
        IConfiguration configuration,
        IPasswordHasher<User> passwordHasher,
        ILogger<AuthController> logger)
    {
        _db = db;
        _configuration = configuration;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    // POST: api/auth/register
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterSuccessResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var normalizedEmail = NormalizeEmail(request.Email);

        var exists = await _db.Users
            .AnyAsync(u => u.Email == normalizedEmail, cancellationToken);

        if (exists)
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Conflict",
                Detail = "Пользователь с таким email уже существует"
            });
        }

        var user = new User
        {
            Name = request.Name.Trim(),
            Email = normalizedEmail
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        _db.Users.Add(user);

        try
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            // Ловим гонку: между AnyAsync и SaveChanges кто-то успел вставить того же email.
            // Работает только если в БД есть уникальный индекс на Email (см. TodoDb).
            _logger.LogWarning(ex, "Конфликт при регистрации пользователя {Email}", normalizedEmail);

            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Conflict",
                Detail = "Пользователь с таким email уже существует"
            });
        }

        var response = new RegisterSuccessResponseDto(
            Message: "Пользователь зарегистрирован",
            UserId: user.Id,
            Email: user.Email);

        return Ok(response);
    }

    // POST: api/auth/login
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginSuccessResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var normalizedEmail = NormalizeEmail(request.Email);

        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == normalizedEmail, cancellationToken);

        if (user is null)
        {
            // Не раскрываем, что именно неверно (email или пароль).
            return Unauthorized(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = "Неверный email или пароль"
            });
        }

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return Unauthorized(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = "Неверный email или пароль"
            });
        }

        if (result == PasswordVerificationResult.SuccessRehashNeeded)
        {
            // Алгоритм/параметры хеширования устарели — обновляем хеш на лету.
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            await _db.SaveChangesAsync(cancellationToken);
        }

        var token = GenerateToken(user);

        var response = new LoginSuccessResponseDto(
            Token: token,
            UserId: user.Id,
            Email: user.Email);

        return Ok(response);
    }

    private string GenerateToken(User user)
    {
        var key = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Конфигурация Jwt:Key не задана.");
        var issuer = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("Конфигурация Jwt:Issuer не задана.");
        var audience = _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("Конфигурация Jwt:Audience не задана.");

        var lifetimeHours = _configuration.GetValue<int?>("Jwt:ExpiresHours")
                            ?? DefaultTokenLifetimeHours;

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(lifetimeHours),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string NormalizeEmail(string email) =>
        email.Trim().ToLowerInvariant();
}