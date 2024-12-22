using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Valhalla_v3.Services;
using Valhalla_v3.Shared;

namespace Valhalla_v3.Controller;
[ApiController]
[Route("api/[controller]")]
public class OperatorController : ControllerBase
{
    private readonly UserManager<Operator> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public OperatorController(UserManager<Operator> userManager, RoleManager<IdentityRole<int>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // GET: api/operator
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Operator>>> Get()
    {
        var users = await _userManager.Users.ToListAsync();
        return Ok(users);
    }

    // GET: api/operator/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Operator>> Get(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
            return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OperatorDto operatorDto)
    {
        if (string.IsNullOrWhiteSpace(operatorDto.UserName) || string.IsNullOrWhiteSpace(operatorDto.Password))
            return BadRequest("Username and Password are required.");

        // Sprawdź unikalność nazwy użytkownika
        var existingUser = await _userManager.FindByNameAsync(operatorDto.UserName);
        if (existingUser != null)
            return Conflict($"A user with the username '{operatorDto.UserName}' already exists.");

        // Sprawdź unikalność adresu e-mail
        if (!string.IsNullOrWhiteSpace(operatorDto.Email))
        {
            var existingEmailUser = await _userManager.FindByEmailAsync(operatorDto.Email);
            if (existingEmailUser != null)
                return Conflict($"A user with the email '{operatorDto.Email}' already exists.");
        }

        // Tworzenie użytkownika
        var operatorUser = new Operator
        {
            UserName = operatorDto.UserName,
            Email = operatorDto.Email,
            Name = operatorDto.UserName,
            Password = operatorDto.Password// Jeśli kolumna Name jest wymagana
        };

        var createUserResult = await _userManager.CreateAsync(operatorUser, operatorDto.Password);

        if (!createUserResult.Succeeded)
            return BadRequest(createUserResult.Errors);
        // Tworzenie roli, jeśli nie istnieje
        if (!string.IsNullOrEmpty(operatorDto.Role))
        {
            if (!await _roleManager.RoleExistsAsync(operatorDto.Role))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole<int> { Name = operatorDto.Role });
                if (!roleResult.Succeeded)
                    return BadRequest(roleResult.Errors);
            }

            // Dodanie użytkownika do roli
            var addToRoleResult = await _userManager.AddToRoleAsync(operatorUser, operatorDto.Role);
            if (!addToRoleResult.Succeeded)
                return BadRequest(addToRoleResult.Errors);
        }

        return CreatedAtAction(nameof(Get), new { id = operatorUser.Id }, operatorUser);
    }



    // DELETE: api/operator/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
            return NotFound();

        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded)
            return NoContent();

        return BadRequest(result.Errors);
    }
}
