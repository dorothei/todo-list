using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Db;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly TodoDb _db;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthController(TodoDb db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
        _passwordHasher = new PasswordHasher<User>();
    }

    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (_db.Users.Any(u => u.Email == request.Email))
        {
            return BadRequest(new { message = "Пользователь с таким email уже существует" });
        }

        var user = new User
        {
            Name = request.Name,
            Email = request.Email
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "Пользователь зарегистрирован",
            userId = user.Id,
            email = user.Email
        });
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == request.Email);

        if (user == null)
        {
            return Unauthorized(new { message = "Неверный email или пароль" });
        }

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return Unauthorized(new { message = "Неверный email или пароль" });
        }

        var token = GenerateToken(user);

        return Ok(new
        {
            token,
            userId = user.Id,
            email = user.Email
        });
    }

    private string GenerateToken(User user)
    {
        var key = _configuration["Jwt:Key"];

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key!));

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class RegisterRequest
{
    public string? Name { get; set; }

    public string Email { get; set; } = "";

    public string Password { get; set; } = "";
}

public class LoginRequest
{
    public string Email { get; set; } = "";

    public string Password { get; set; } = "";
}