using Microsoft.AspNetCore.SignalR;
using Valhalla_v3.Services.CarHistory;

namespace Valhalla_v3.Hubs;

public class CarHub : Hub
{
    private readonly ICarService _CarService;

    public CarHub(ICarService carService)
    {
        _CarService = carService;
    }

    public async Task SendMessage()
    {
        var CarList = _CarService.Get();
        await Clients.All.SendAsync("CarList", CarList);
    }

    public async Task GetCar(string id)
    {
        if (!int.TryParse(id, out var carId))
        {
            await Clients.All.SendAsync("Car", null);
        }
        var Car = _CarService.Get(carId);
        await Clients.All.SendAsync("Car", Car);
    }
}
