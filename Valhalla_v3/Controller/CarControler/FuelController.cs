using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
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
    [Authorize]
    public async Task<ActionResult<IEnumerable<CarHistoryFuel>>> Get()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Brak sub w tokenie.");

            if (!int.TryParse(userId, out int Userid))
                return Unauthorized("Błąd w przekazanym id");
            var fuels = await _carHistoryFuelService.Get(Userid);
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
    [Authorize]
    public async Task<ActionResult> Create([FromBody] CarHistoryFuel fuel)
    {
        if (fuel == null)
            return BadRequest(new { message = "Fuel object cannot be null." });

        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Brak sub w tokenie.");

            if (!int.TryParse(userId, out int Userid))
                return Unauthorized("Błąd w przekazanym id");
            if (fuel.Id != 0)
            {
                await _carHistoryFuelService.Update(fuel, Userid);
                return Ok(new { message = "Fuel history updated successfully." }); 
            }
            else
            {
                fuel.Id = await _carHistoryFuelService.Create(fuel, Userid);
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
            var json = JsonSerializer.Serialize(fuel);
            return StatusCode(500, new { message = $"An unexpected error occurred while creating fuel history." });
        }
    }
}

