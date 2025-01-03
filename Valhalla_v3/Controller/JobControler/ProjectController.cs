using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Valhalla_v3.Services.ToDo;
using Valhalla_v3.Shared.ToDo;

namespace Valhalla_v3.Controller;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Project>>> Get()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Brak sub w tokenie.");

            if (!int.TryParse(userId, out int Userid))
                return Unauthorized("Błąd w przekazanym id");
            var projects = await _projectService.Get(Userid);
            if (projects == null || !projects.Any())
                return NoContent(); 

            return Ok(projects); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while retrieving projects." });
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Project>> Get(int id)
    {
        if (id <= 0)
            return BadRequest(new { message = "Invalid ID. ID must be greater than zero." });
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("Brak sub w tokenie.");

        if (!int.TryParse(userId, out int Userid))
            return Unauthorized("Błąd w przekazanym id");
        try
        {
            var project = await _projectService.Get(id, Userid);
            if (project == null)
                return NotFound(new { message = $"Project with ID {id} not found." });

            return Ok(project); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while retrieving the project." });
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] Project project)
    {
        if (project == null)
            return BadRequest(new { message = "Project object cannot be null." });
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("Brak sub w tokenie.");

        if (!int.TryParse(userId, out int Userid))
            return Unauthorized("Błąd w przekazanym id");
        try
        {
            if (project.Id != 0)
            {
                await _projectService.Update(project, Userid);
                return Ok(new { message = "Project updated successfully." }); 
            }
            else
            {
                project.Id = await _projectService.Create(project, Userid);
                return CreatedAtAction(nameof(Create), new { id = project.Id }, project); 
            }
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while creating or updating the project." });
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> Delete(int id)
    {
        if (id <= 0)
            return BadRequest(new { message = "Invalid ID. ID must be greater than zero." });
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("Brak sub w tokenie.");

        if (!int.TryParse(userId, out int Userid))
            return Unauthorized("Błąd w przekazanym id");
        try
        {
            var project = await _projectService.Get(id, Userid);
            if (project == null)
                return NotFound(new { message = $"Project with ID {id} not found." });

            await _projectService.Delete(id, Userid);
            return Ok(new { message = $"Project with ID {id} deleted successfully." }); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while deleting the project." });
        }
    }
}

