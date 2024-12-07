using Microsoft.AspNetCore.Mvc;
using Valhalla_v3.Services;
using Valhalla_v3.Shared;


namespace Valhalla_v3.Controller;

[ApiController]
[Route("api/[controller]")]
public class OperatorController : ControllerBase
{
    private readonly IOperatorService _operatorService;

    public OperatorController(IOperatorService operatorService)
    {
        _operatorService = operatorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Operator>>> Get()
    {
        return await _operatorService.Get();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Operator>> Get(int id)
    {
        if (id == 0)
            return NotFound();
        return await _operatorService.Get(id);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Operator oper)
    {
        try
        {
            if (oper.Id != 0)
                await _operatorService.Update(oper);
            else
                oper.Id = await _operatorService.Create(oper);
            return CreatedAtAction("Create", oper.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw ex;
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if (id == 0)
            return NotFound();
        await _operatorService.Delete(id);
        return CreatedAtAction("Delete", id);

    }
}