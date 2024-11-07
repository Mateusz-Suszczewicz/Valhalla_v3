using Microsoft.AspNetCore.Mvc;
using Valhalla_v3.Services.CarHistory;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Controller;

[ApiController]
[Route("api/[controller]")]
public class MechanicController : ControllerBase
{
    private readonly IMechanicService _MechanicService;

    public MechanicController(IMechanicService MechanicService)
    {
        _MechanicService = MechanicService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Mechanic>>> Get()
    {
        return await _MechanicService.Get();
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Mechanic fuel)
    {
        if (fuel == null)
        {
            return BadRequest();
        }
        var id = await _MechanicService.Create(fuel);

        return CreatedAtAction("Create", id);
    }

}
