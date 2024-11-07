﻿using Microsoft.AspNetCore.Mvc;
using Valhalla_v3.Services.CarHistory;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Controller;

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
        return await _carService.Get();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Car>> Get(int id)
    {
        if(id == 0)
            return NotFound();
        return await _carService.Get(id);
    }
}
