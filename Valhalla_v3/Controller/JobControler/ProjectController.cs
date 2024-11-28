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
        return await _projectService.Get();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Project>> Get(int id)
    {
        if (id == 0)
            return NotFound();
        return await _projectService.Get(id);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Project project)
    {
        try
        {
            if (project.Id != 0)
                await _projectService.Update(project);
            else
                project.Id = await _projectService.Create(project);
            return CreatedAtAction("Create", project.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw ex;
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if (id == 0)
            return NotFound();
        await _projectService.Delete(id);
        return CreatedAtAction("Delete", id);

    }
}
