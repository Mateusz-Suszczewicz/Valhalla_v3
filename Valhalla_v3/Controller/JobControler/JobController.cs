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
    public async Task<ActionResult<IEnumerable<Job>>> Get()
    {
        return await _jobService.Get();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Job>> Get(int id)
    {
        if(id == 0)
            return NotFound();
        return await _jobService.Get(id);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Job car)
    {
        try
        {
            if (car.Id != 0)
                await _jobService.Update(car);
            else
                car.Id = await _jobService.Create(car);
            return CreatedAtAction("Create", car.Id);
        }
        catch(Exception ex)
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
        await _jobService.Delete(id);
        return CreatedAtAction("Delete", id);

    }
}
