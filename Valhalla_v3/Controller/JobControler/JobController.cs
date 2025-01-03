using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Valhalla_v3.Services.ToDo;
using Valhalla_v3.Shared.ToDo;

namespace Valhalla_v3.Controller;

[ApiController]
[Route("api/[controller]")]
public class JobController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Job>>> Get([FromQuery] bool NoDoneJobs, [FromQuery] int ProjectId)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Brak sub w tokenie.");

            if (!int.TryParse(userId, out int Userid))
                return Unauthorized("Błąd w przekazanym id");
            var jobs = await _jobService.Get(NoDoneJobs, ProjectId, Userid);
            if (jobs == null || !jobs.Any())
                return NoContent();

            return Ok(jobs);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while retrieving jobs." });
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Job>> Get(int id)
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
            var job = await _jobService.Get(id, Userid);
            if (job == null)
                return NotFound(new { message = $"Job with ID {id} not found." });

            return Ok(job);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while retrieving the job." });
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] Job job)
    {
        if (job == null)
            return BadRequest(new { message = "Job object cannot be null." });
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("Brak sub w tokenie.");

        if (!int.TryParse(userId, out int Userid))
            return Unauthorized("Błąd w przekazanym id");
        try
        {
            if (job.Id != 0)
            {
                await _jobService.Update(job, Userid);
                return Ok(new { message = "Job updated successfully." });
            }
            else
            {
                job.Id = await _jobService.Create(job, Userid);
                return CreatedAtAction(nameof(Create), new { id = job.Id }, job);
            }
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while creating or updating the job." });
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
            var job = await _jobService.Get(id, Userid);
            if (job == null)
                return NotFound(new { message = $"Job with ID {id} not found." });

            await _jobService.Delete(id, Userid);
            return Ok(new { message = $"Job with ID {id} deleted successfully." }); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while deleting the job." });
        }
    }

    [HttpPost("changeTerm")]
    [Authorize]
    public async Task<ActionResult> ChangeTerm([FromQuery] int Id, DateTime newTerm)
    {
        if (Id <= 0)
            return BadRequest(new { message = "Invalid ID. ID must be greater than zero." });
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("Brak sub w tokenie.");

        if (!int.TryParse(userId, out int Userid))
            return Unauthorized("Błąd w przekazanym id");
        try
        {
            await _jobService.ChangeTerm(Id, newTerm, Userid);
            
            return Ok(new { message = $"Job with ID {Id} changed successfully." });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while changed the term." });
        }
    }
}
