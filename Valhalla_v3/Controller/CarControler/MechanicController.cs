using Microsoft.AspNetCore.Mvc;
using Valhalla_v3.Services.CarHistory;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Controller.CarControler;

[ApiController]
[Route("api/[controller]")]
public class MechanicController : ControllerBase
{
    private readonly IMechanicService _mechanicService;

    public MechanicController(IMechanicService mechanicService)
    {
        _mechanicService = mechanicService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Mechanic>>> Get()
    {
        try
        {
            var mechanics = await _mechanicService.Get();
            if (mechanics == null || !mechanics.Any())
                return NoContent(); 

            return Ok(mechanics); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while retrieving mechanics." });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Mechanic>> Get(int id)
    {
        if (id <= 0)
            return BadRequest(new { message = "Invalid ID. ID must be greater than zero." });

        try
        {
            var mechanic = await _mechanicService.Get(id);
            if (mechanic == null)
                return NotFound(new { message = $"Mechanic with ID {id} not found." });

            return Ok(mechanic);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = $"An unexpected error occurred while retrieving the mechanic.{ex.InnerException}" });
        }
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Mechanic mechanic)
    {
        if (mechanic == null)
        {
            return BadRequest(new { message = "Mechanic object cannot be null." });
        }

        try
        {
            var id = await _mechanicService.Create(mechanic);
            return CreatedAtAction(nameof(Create), new { id }, mechanic); 
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = $"An unexpected error occurred while creating the mechanic. {ex.InnerException}" });
        }
    }
}

