using ToTask.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToTask.DTOs;
using ToTask.Models;
using ToTask.Services;

namespace ToTask.Controllers;

[ApiController]
[Route("user/")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<UserDTO>> GetSelf()
    {
        int id = this.GetUserId();
        UserDTO user = await _userService.GetUserById(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDTO>> AddUser(CreateUserDTO userDTO)
    {
        User newUser = await _userService.AddUser(userDTO);
        return CreatedAtAction(nameof(GetSelf), new { }, await _userService.GetUserById(newUser.Id));
    }
    
    [HttpDelete]
    [Authorize]
    public async Task<ActionResult> DeleteUser()
    {
        int id = this.GetUserId();
        bool deleted = await _userService.DeleteUser(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}