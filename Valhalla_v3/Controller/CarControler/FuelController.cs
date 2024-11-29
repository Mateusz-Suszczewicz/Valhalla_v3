using Microsoft.AspNetCore.Mvc;
using Valhalla_v3.Services.CarHistory;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Controller.CarControler;

[ApiController]
[Route("api/[controller]")]
public class FuelController : ControllerBase
{
    private readonly ICarHistoryFuelService _CarHistoryFuelService;

    public FuelController(ICarHistoryFuelService CarHistoryFuelService)
    {
        _CarHistoryFuelService = CarHistoryFuelService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarHistoryFuel>>> Get()
    {
        return await _CarHistoryFuelService.Get();
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CarHistoryFuel fuel)
    {
        try
        {
            if (fuel.Id != 0)
                await _CarHistoryFuelService.Update(fuel);
            else
                fuel.Id = await _CarHistoryFuelService.Create(fuel);
            return CreatedAtAction("Create", fuel.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw ex;
        }

    }
}
