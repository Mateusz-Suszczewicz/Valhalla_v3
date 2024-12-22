using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Valhalla_v3.Controller;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public RoleController(RoleManager<IdentityRole<int>> roleManager)
    {
        _roleManager = roleManager;
    }

    // POST: api/role
    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return BadRequest("Role name cannot be empty.");

        // Sprawdź, czy rola już istnieje
        if (await _roleManager.RoleExistsAsync(roleName))
            return Conflict($"Role '{roleName}' already exists.");

        // Tworzenie roli
        var role = new IdentityRole<int> { Name = roleName };
        var result = await _roleManager.CreateAsync(role);

        if (result.Succeeded)
            return Ok($"Role '{roleName}' created successfully.");

        return BadRequest(result.Errors);
    }

    // GET: api/role
    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = _roleManager.Roles.ToList();
        return Ok(roles);
    }
}

