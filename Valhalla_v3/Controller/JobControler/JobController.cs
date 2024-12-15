using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<IEnumerable<Job>>> Get([FromQuery] bool DoneJobs)
    {
        try
        {
            var jobs = await _jobService.Get(DoneJobs);
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
    public async Task<ActionResult<Job>> Get(int id)
    {
        if (id <= 0)
            return BadRequest(new { message = "Invalid ID. ID must be greater than zero." });

        try
        {
            var job = await _jobService.Get(id);
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
    public async Task<IActionResult> Create([FromBody] Job job)
    {
        if (job == null)
            return BadRequest(new { message = "Job object cannot be null." });

        try
        {
            if (job.Id != 0)
            {
                await _jobService.Update(job);
                return Ok(new { message = "Job updated successfully." });
            }
            else
            {
                job.Id = await _jobService.Create(job);
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
    public async Task<ActionResult> Delete(int id)
    {
        if (id <= 0)
            return BadRequest(new { message = "Invalid ID. ID must be greater than zero." });

        try
        {
            var job = await _jobService.Get(id);
            if (job == null)
                return NotFound(new { message = $"Job with ID {id} not found." });

            await _jobService.Delete(id);
            return Ok(new { message = $"Job with ID {id} deleted successfully." }); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while deleting the job." });
        }
    }
}
