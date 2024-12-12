using Microsoft.AspNetCore.Mvc;
using Valhalla_v3.Services.CarHistory;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Controller.CarControler;

[ApiController]
[Route("api/[controller]")]
public class FuelController : ControllerBase
{
    private readonly ICarHistoryFuelService _carHistoryFuelService;

    public FuelController(ICarHistoryFuelService carHistoryFuelService)
    {
        _carHistoryFuelService = carHistoryFuelService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarHistoryFuel>>> Get()
    {
        try
        {
            var fuels = await _carHistoryFuelService.Get();
            if (fuels == null || !fuels.Any())
                return NoContent(); 

            return Ok(fuels);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while retrieving fuel history." });
        }
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CarHistoryFuel fuel)
    {
        if (fuel == null)
            return BadRequest(new { message = "Fuel object cannot be null." });

        try
        {
            if (fuel.Id != 0)
            {
                await _carHistoryFuelService.Update(fuel);
                return Ok(new { message = "Fuel history updated successfully." }); 
            }
            else
            {
                fuel.Id = await _carHistoryFuelService.Create(fuel);
                return CreatedAtAction(nameof(Create), new { id = fuel.Id }, fuel); 
            }
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while creating fuel history." });
        }
    }
}

