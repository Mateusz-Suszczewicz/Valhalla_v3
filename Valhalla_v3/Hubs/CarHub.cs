using Microsoft.AspNetCore.SignalR;
using Valhalla_v3.Services.CarHistory;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Hubs;

public class CarHub : Hub
{
    private readonly ICarService _CarService;
    private readonly IGasStationService _GasStationService;
    private readonly ICarHistoryFuelService _CarHistoryFuelService;
    private readonly ICarHistoryRepairService _CarHistoryRepairService;
    private readonly IMechanicService _MechanicService;

    public CarHub(ICarService carService, IGasStationService GasStationService, ICarHistoryFuelService carHistoryFuelService, IMechanicService mechanicService, ICarHistoryRepairService carHistoryRepairService)
    {
        _CarService = carService;
        _GasStationService = GasStationService;
        _CarHistoryFuelService = carHistoryFuelService;
        _MechanicService = mechanicService;
        _CarHistoryRepairService = carHistoryRepairService;
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
            await Clients.All.SendAsync("Car", null);
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

    public async Task GetMechanic()
    {
        var Car = _MechanicService.Get();
        await Clients.All.SendAsync("Mechanics", Car);
    }

    public async Task AddRepair(CarHistoryRepair repair)
    {
        var id = await _CarHistoryRepairService.Create(repair);
        await Clients.All.SendAsync("AddedRepair", id);
    }

    public async Task AddGasStation(GasStation repair)
    {
        var id = await _GasStationService.Create(repair);
        await Clients.All.SendAsync("AddedGasStation", id);
    }
}
