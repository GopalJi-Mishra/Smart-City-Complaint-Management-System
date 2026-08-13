using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartCityComplaint.Data;
using SmartCityComplaint.Models;

namespace SmartCityComplaint.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<User>();
    }

    [HttpPost("register")]
    public IActionResult Register(User user)
    {
        var existingUser = _context.Users
            .FirstOrDefault(u => u.Email == user.Email);

        if (existingUser != null)
        {
            return BadRequest("Email already registered");
        }

        user.Password = _passwordHasher.HashPassword(
            user,
            user.Password
        );

        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok("Registration successful");
    }

    [HttpPost("login")]
    public IActionResult Login(User user)
    {
        var existingUser = _context.Users
            .FirstOrDefault(u => u.Email == user.Email);

        if (existingUser == null)
        {
            return Unauthorized("Invalid email or password");
        }

        var result = _passwordHasher.VerifyHashedPassword(
            existingUser,
            existingUser.Password,
            user.Password
        );

        if (result == PasswordVerificationResult.Failed)
        {
            return Unauthorized("Invalid email or password");
        }

        return Ok(new
        {
            message = "Login successful",
            role = existingUser.Role,
            userId = existingUser.Id
        });
    }
}