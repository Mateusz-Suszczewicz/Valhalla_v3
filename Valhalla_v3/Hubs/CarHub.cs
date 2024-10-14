using Microsoft.AspNetCore.SignalR;
using Valhalla_v3.Services.CarHistory;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Hubs;

public class CarHub : Hub
{
    private readonly ICarService _CarService;
    private readonly IGasStationService _GasStationService;
    private readonly ICarHistoryFuelService _CarHistoryFuelService;

    public CarHub(ICarService carService, IGasStationService GasStationService, ICarHistoryFuelService carHistoryFuelService)
    {
        _CarService = carService;
        _GasStationService = GasStationService;
        _CarHistoryFuelService = carHistoryFuelService;
    }

    public async Task SendMessage()
    {
        var CarList = _CarService.Get();
        await Clients.All.SendAsync("CarList", CarList);
    }

    public async Task GetCar(string id)
    {
        try
        {
            if (!int.TryParse(id, out var carId))
            {
                await Clients.All.SendAsync("Car", null);
                return;
            }

            var Car = _CarService.Get(carId);
            await Clients.All.SendAsync("Car", Car);
        }
        catch (Exception ex)
        {
            // Loguj wyjątek
            Console.WriteLine(ex.Message);
            await Clients.All.SendAsync("Car", null); // Wyślij komunikat o błędzie
        }
    }

    public async Task GetGasStation()
    {
        var Car = _GasStationService.Get();
        await Clients.All.SendAsync("GasStation", Car);
    }

    public async Task AddFuel(CarHistoryFuel fuel)
    {
        var id = await _CarHistoryFuelService.Create(fuel);
        await Clients.All.SendAsync("AddedFuel", id);
    }
}
