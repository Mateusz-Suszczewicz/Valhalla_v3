using Microsoft.AspNetCore.SignalR;
using Valhalla_v3.Client.Services;
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
}
