using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
    [Authorize]
    public async Task<ActionResult<IEnumerable<GasStation>>> Get()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Brak sub w tokenie.");

            if (!int.TryParse(userId, out int Userid))
                return Unauthorized("Błąd w przekazanym id"); 
            
            var gasStations = await _gasStationService.Get(Userid);
            
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
    
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<GasStation>> Get(int id)
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
            var gasStation = await _gasStationService.Get(id, Userid);
            if (gasStation == null)
                return NotFound(new { message = $"Gas Station with ID {id} not found." });

            return Ok(gasStation);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = $"An unexpected error occurred while retrieving the gas station.{ex.Message}" });
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] GasStation gasStation)
    {
        if (gasStation == null)
            return BadRequest(new { message = "Gas station object cannot be null." });
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("Brak sub w tokenie.");

        if (!int.TryParse(userId, out int Userid))
            return Unauthorized("Błąd w przekazanym id");
        try
        {
            if (gasStation.Id != 0)
            {
                await _gasStationService.Update(gasStation, Userid);
                return Ok(new { message = "Gas station updated successfully." }); 
            }
            else
            {
                gasStation.Id = await _gasStationService.Create(gasStation, Userid);
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
            return StatusCode(500, new { message = $"An unexpected error occurred while creating or updating the gas station. {ex.InnerException}" });
        }
    }
}

