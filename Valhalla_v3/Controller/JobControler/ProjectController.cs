using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<IEnumerable<Project>>> Get()
    {
        try
        {
            var projects = await _projectService.Get();
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
    public async Task<ActionResult<Project>> Get(int id)
    {
        if (id <= 0)
            return BadRequest(new { message = "Invalid ID. ID must be greater than zero." });

        try
        {
            var project = await _projectService.Get(id);
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
    public async Task<IActionResult> Create([FromBody] Project project)
    {
        if (project == null)
            return BadRequest(new { message = "Project object cannot be null." });

        try
        {
            if (project.Id != 0)
            {
                await _projectService.Update(project);
                return Ok(new { message = "Project updated successfully." }); 
            }
            else
            {
                project.Id = await _projectService.Create(project);
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
    public async Task<ActionResult> Delete(int id)
    {
        if (id <= 0)
            return BadRequest(new { message = "Invalid ID. ID must be greater than zero." });

        try
        {
            var project = await _projectService.Get(id);
            if (project == null)
                return NotFound(new { message = $"Project with ID {id} not found." });

            await _projectService.Delete(id);
            return Ok(new { message = $"Project with ID {id} deleted successfully." }); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while deleting the project." });
        }
    }
}

