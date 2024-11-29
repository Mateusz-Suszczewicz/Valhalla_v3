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
        return await _gasStationService.Get();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GasStation gasStation)
    {
        try
        {
            if (gasStation.Id != 0)
                await _gasStationService.Update(gasStation);
            else
                gasStation.Id = await _gasStationService.Create(gasStation);
            return CreatedAtAction("Create", gasStation.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw ex;
        }
    }

}
