using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Valhalla_v3.Services.CarHistory;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Controller.CarControler;

[ApiController]
[Route("api/[controller]")]
public class CarController : ControllerBase
{
    private readonly ICarService _carService;

    public CarController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Car>>> Get()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Brak sub w tokenie.");
            }
            if(!int.TryParse(userId, out int id))
                return Unauthorized("Błąd w przekazanym id");
            var cars = await _carService.Get(id);
            if (cars == null || !cars.Any())
                return NoContent(); 

            return Ok(cars); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while retrieving cars." });
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Car>> Get(int id)
    {
        if (id <= 0)
            return BadRequest(new { message = "Invalid ID. ID must be greater than zero." });

        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Brak sub w tokenie.");
            }
            if (!int.TryParse(userId, out int Userid))
                return Unauthorized("Błąd w przekazanym id");

            var car = await _carService.Get(id, Userid);
            if (car == null)
                return NotFound(new { message = $"Car with ID {id} not found." });

            return Ok(car);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while retrieving the car." });
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> Delete(int id)
    {
        if (id <= 0)
            return BadRequest(new { message = "Invalid ID. ID must be greater than zero." });

        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Brak sub w tokenie.");
            
            if (!int.TryParse(userId, out int Userid))
                return Unauthorized("Błąd w przekazanym id");

            var car = await _carService.Get(id, Userid);
            if (car == null)
                return NotFound(new { message = $"Car with ID {id} not found." });

            await _carService.Delete(id, Userid);
            return Ok(new { message = $"Car with ID {id} deleted successfully." });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while deleting the car." });
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<int>> Create([FromBody] Car car)
    {
        try
        {
            if (car == null)
                return BadRequest(new { message = "Car object cannot be null." });
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Brak sub w tokenie.");

            if (!int.TryParse(userId, out int Userid))
                return Unauthorized("Błąd w przekazanym id");
            if (car.Id != 0)
            {
                await _carService.Update(car, Userid);
                return Ok(new { message = "Car updated successfully." });
            }
            else
            {
                car.Id = await _carService.Create(car, Userid);
                return CreatedAtAction(nameof(Create), new { id = car.Id }, car);
            }
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });
        }
    }
}
