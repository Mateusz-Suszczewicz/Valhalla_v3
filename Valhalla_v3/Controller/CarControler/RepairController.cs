using Microsoft.AspNetCore.Mvc;
using Valhalla_v3.Services.CarHistory;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Controller.CarControler;

[ApiController]
[Route("api/[controller]")]
public class RepairController : ControllerBase
{
    private readonly ICarHistoryRepairService _carHistoryRepairService;

    public RepairController(ICarHistoryRepairService carHistoryRepairService)
    {
        _carHistoryRepairService = carHistoryRepairService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarHistoryRepair>>> Get()
    {
        try
        {
            var repairs = await _carHistoryRepairService.Get();
            if (repairs == null || !repairs.Any())
                return NoContent(); 

            return Ok(repairs); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while retrieving repair history." });
        }
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CarHistoryRepair repair)
    {
        if (repair == null)
        {
            return BadRequest(new { message = "Repair object cannot be null." });
        }

        try
        {
            var id = await _carHistoryRepairService.Create(repair);
            return CreatedAtAction(nameof(Create), new { id }, repair); 
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, new { message = "An unexpected error occurred while creating the repair history." });
        }
    }
}
