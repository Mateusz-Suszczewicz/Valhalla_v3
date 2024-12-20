using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<IEnumerable<Car>>> Get()
    {
        try
        {
            var cars = await _carService.Get();
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
    public async Task<ActionResult<Car>> Get(int id)
    {
        if (id <= 0)
            return BadRequest(new { message = "Invalid ID. ID must be greater than zero." });

        try
        {
            var car = await _carService.Get(id);
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
    public async Task<ActionResult> Delete(int id)
    {
        if (id <= 0)
            return BadRequest(new { message = "Invalid ID. ID must be greater than zero." });

        try
        {
            var car = await _carService.Get(id);
            if (car == null)
                return NotFound(new { message = $"Car with ID {id} not found." });

            await _carService.Delete(id);
            return Ok(new { message = $"Car with ID {id} deleted successfully." });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while deleting the car." });
        }
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] Car car)
    {
        try
        {
            if (car == null)
                return BadRequest(new { message = "Car object cannot be null." });

            if (car.Id != 0)
            {
                await _carService.Update(car);
                return Ok(new { message = "Car updated successfully." });
            }
            else
            {
                car.Id = await _carService.Create(car);
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
