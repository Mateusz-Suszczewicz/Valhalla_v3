using Microsoft.AspNetCore.Mvc;
using Valhalla_v3.Services.CarHistory;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Controller;

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
        return await _carHistoryRepairService.Get();
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CarHistoryRepair fuel)
    {
        if (fuel == null)
        {
            return BadRequest();
        }
        var id = await _carHistoryRepairService.Create(fuel);

        return CreatedAtAction("Create", id);
    }
}
