using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToTask.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ToTask.Data;
using ToTask.Models;
using ToTask.Utils;

namespace ToTask.Controllers;

[ApiController]
[Route("jwt/")]
public class JwtController : ControllerBase
{
    private readonly JwtSettings _jwtSettings;
    private readonly IUserRepository _userRepository;

    public JwtController(JwtSettings jwtSettings, IUserRepository userRepository)
    {
        _jwtSettings = jwtSettings;
        _userRepository = userRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest model)
    {
        var user = await _userRepository.GetUserByUsername(model.Username);

        if (user == null)
        {
            return Unauthorized();
        }

        if (!VerifyPassword(model.Password, user.Password))
        {
            return Unauthorized();
        }

        var token = GenerateJwtToken(user);
        return Ok(new { Token = token });
    }

    private string GenerateJwtToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
    
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
    
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    private bool VerifyPassword(string enteredPassword, string storedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPassword);
    }
}