using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lektion_12;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private static List<UserEntity> Users = new();
    public static UserEntity CurrentUser = new();

    [HttpPost("register")]
    public ActionResult<UserDto> Register([FromBody] UserDto user)
    {
        bool hasDigit = Regex.IsMatch(user.Password, @"[0-9]");

        bool hasCapital = Regex.IsMatch(user.Password, @"[A-Z]");

        bool hasSpecial = Regex.IsMatch(user.Password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]");

        if (user.Password.Length > 8 && hasDigit && hasCapital && hasSpecial)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password, workFactor: 12);

            Users.Add(new UserEntity { Username = user.Username, PasswordHash = hashedPassword });

            return Ok();
        }

        return BadRequest("Do better");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserDto user)
    {
        var existingUser = Users.FirstOrDefault(u => u.Username == user.Username);
        if (
            existingUser == null
            || !BCrypt.Net.BCrypt.Verify(user.Password, existingUser.PasswordHash)
        )
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        CurrentUser = existingUser;

        return Ok(new { message = "Logged in successful", username = user.Username });
    }
}
