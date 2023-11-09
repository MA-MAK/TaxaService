using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserRepository _userRepository;

    public UserController(UserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        var users = await _userRepository.GetAllUsers();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(string id)
    {
        var user = await _userRepository.GetUserById(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody] User user)
    {
        await _userRepository.InsertUser(user);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(string id, [FromBody] User updatedUser)
    {
        var existingUser = await _userRepository.GetUserById(id);

        if (existingUser == null)
        {
            return NotFound();
        }

        await _userRepository.UpdateUser(id, updatedUser);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(string id)
    {
        var existingUser = await _userRepository.GetUserById(id);

        if (existingUser == null)
        {
            return NotFound();
        }

        await _userRepository.DeleteUser(id);
        return NoContent();
    }
}