using Microsoft.EntityFrameworkCore;
using Valhalla_v3.Database;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Services.CarHistory;

public interface ICarHistoryFuelService
{
    Task<int> Create(CarHistoryFuel fuel);
    Task<CarHistoryFuel> Get(int id);
    Task<List<CarHistoryFuel>> Get();
    Task Update(CarHistoryFuel fuel);
    Task Delete(int id);
}

public class CarHistoryFuelService : ICarHistoryFuelService
{
    private readonly ValhallaContext _context;

    public CarHistoryFuelService(ValhallaContext context)
    {
        _context = context;
    }

    public async Task<int> Create(CarHistoryFuel fuel)
    {
        if (fuel == null)
            throw new ArgumentNullException(nameof(fuel), "Fuel object cannot be null.");

        if (fuel.Id != 0)
            throw new ArgumentException("Fuel ID must be 0 for a new entry.");

        fuel.DateTimeAdd = DateTime.Now;
        fuel.DateTimeModify = DateTime.Now;

        await _context.CarHistoryFuels.AddAsync(fuel);
        await _context.SaveChangesAsync();

        return fuel.Id;
    }

    public async Task Delete(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var fuel = await _context.CarHistoryFuels.FirstOrDefaultAsync(x => x.Id == id);

        if (fuel == null)
            throw new KeyNotFoundException($"Fuel with ID {id} not found.");

        _context.CarHistoryFuels.Remove(fuel);
        await _context.SaveChangesAsync();
    }

    public async Task<CarHistoryFuel> Get(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var fuel = await _context.CarHistoryFuels
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .Include(x => x.GasStation)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (fuel == null)
            throw new KeyNotFoundException($"Fuel with ID {id} not found.");

        return fuel;
    }

    public async Task<List<CarHistoryFuel>> Get()
    {
        var fuelList = await _context.CarHistoryFuels
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .Include(x => x.GasStation)
            .ToListAsync();

        return fuelList ?? new List<CarHistoryFuel>();
    }

    public async Task Update(CarHistoryFuel fuel)
    {
        if (fuel == null)
            throw new ArgumentNullException(nameof(fuel), "Fuel object cannot be null.");

        if (fuel.Id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var existingFuel = await _context.CarHistoryFuels.FirstOrDefaultAsync(x => x.Id == fuel.Id);

        if (existingFuel == null)
            throw new KeyNotFoundException($"Fuel with ID {fuel.Id} not found.");

        existingFuel.Mileage = fuel.Mileage;
        existingFuel.Date = fuel.Date;
        existingFuel.Cost = fuel.Cost;
        existingFuel.CostPerLitr = fuel.CostPerLitr;
        existingFuel.GasStation = fuel.GasStation;
        existingFuel.DateTimeModify = DateTime.Now;

        await _context.SaveChangesAsync();
    }
}

