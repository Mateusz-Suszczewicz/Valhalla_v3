using Microsoft.AspNetCore.Mvc;
using Valhalla_v3.Services.CarHistory;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Controller.CarControler;

[ApiController]
[Route("api/[controller]")]
public class GasStationController : ControllerBase
{
    private readonly IGasStationService _gasStationService;

    public GasStationController(IGasStationService gasStationService)
    {
        _gasStationService = gasStationService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GasStation>>> Get()
    {
        try
        {
            var gasStations = await _gasStationService.Get();
            if (gasStations == null || !gasStations.Any())
                return NoContent(); 

            return Ok(gasStations); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while retrieving gas stations." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GasStation gasStation)
    {
        if (gasStation == null)
            return BadRequest(new { message = "Gas station object cannot be null." });

        try
        {
            if (gasStation.Id != 0)
            {
                await _gasStationService.Update(gasStation);
                return Ok(new { message = "Gas station updated successfully." }); 
            }
            else
            {
                gasStation.Id = await _gasStationService.Create(gasStation);
                return CreatedAtAction(nameof(Create), new { id = gasStation.Id }, gasStation); 
            }
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while creating or updating the gas station." });
        }
    }
}

