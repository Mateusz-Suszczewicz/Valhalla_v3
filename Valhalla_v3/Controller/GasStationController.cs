using Microsoft.AspNetCore.Mvc;
using Valhalla_v3.Services.CarHistory;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Controller;

[ApiController]
[Route("api/[controller]")]
public class GasStationController: ControllerBase
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
}
