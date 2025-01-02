using Microsoft.EntityFrameworkCore;
using Valhalla_v3.Database;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Services.CarHistory;

public interface ICarHistoryFuelService
{
    Task<int> Create(CarHistoryFuel fuel, int userId);
    Task<CarHistoryFuel> Get(int id, int userId);
    Task<List<CarHistoryFuel>> Get(int userId);
    Task Update(CarHistoryFuel fuel, int userId);
    Task Delete(int id, int userId);
}

public class CarHistoryFuelService : ICarHistoryFuelService
{
    private readonly ValhallaContext _context;

    public CarHistoryFuelService(ValhallaContext context)
    {
        _context = context;
    }

    public async Task<int> Create(CarHistoryFuel fuel, int userId)
    {
        if (fuel == null)
            throw new ArgumentNullException(nameof(fuel), "Fuel object cannot be null.");

        if (fuel.Id != 0)
            throw new ArgumentException("Fuel ID must be 0 for a new entry.");
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        if (!validMileage(fuel.Mileage, fuel.CarId))
            throw new ArgumentException("Mileage must be greater than saved mileage.");

        fuel.DateTimeAdd = DateTime.Now;
        fuel.DateTimeModify = DateTime.Now;
        fuel.OperatorModifyId = userId;
        fuel.OperatorCreateId = userId;
        await _context.CarHistoryFuels.AddAsync(fuel);
        await _context.SaveChangesAsync();

        return fuel.Id;
    }

    public async Task Delete(int id, int userId)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        var fuel = await _context.CarHistoryFuels.FirstOrDefaultAsync(x => x.Id == id && x.OperatorModifyId == userId);

        if (fuel == null)
            throw new KeyNotFoundException($"Fuel with ID {id} not found.");

        _context.CarHistoryFuels.Remove(fuel);
        await _context.SaveChangesAsync();
    }

    public async Task<CarHistoryFuel> Get(int id, int userId)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        var fuel = await _context.CarHistoryFuels
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .Include(x => x.GasStation)
            .FirstOrDefaultAsync(x => x.Id == id && x.OperatorModifyId == userId);

        if (fuel == null)
            throw new KeyNotFoundException($"Fuel with ID {id} not found.");

        return fuel;
    }

    public async Task<List<CarHistoryFuel>> Get(int userId)
    {
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        var fuelList = await _context.CarHistoryFuels
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .Include(x => x.GasStation)
            .Where(x => x.OperatorModifyId == userId)
            .ToListAsync();

        return fuelList ?? new List<CarHistoryFuel>();
    }

    public async Task Update(CarHistoryFuel fuel, int userId)
    {
        if (fuel == null)
            throw new ArgumentNullException(nameof(fuel), "Fuel object cannot be null.");

        if (fuel.Id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        if (!validMileage(fuel.Mileage, fuel.CarId))
            throw new ArgumentException("Mileage must be greater than saved mileage.");

        var existingFuel = await _context.CarHistoryFuels.FirstOrDefaultAsync(x => x.Id == fuel.Id && x.OperatorModifyId == userId);

        if (existingFuel == null)
            throw new KeyNotFoundException($"Fuel with ID {fuel.Id} not found.");

        existingFuel.Mileage = fuel.Mileage;
        existingFuel.Date = fuel.Date;
        existingFuel.Cost = fuel.Cost;
        existingFuel.CostPerLitr = fuel.CostPerLitr;
        existingFuel.GasStation = fuel.GasStation;
        existingFuel.DateTimeModify = DateTime.Now;
        existingFuel.OperatorModifyId = userId;
        await _context.SaveChangesAsync();
    }

    private bool validMileage(int newMileage, int carId)
    {
        var fuelMileage = _context.CarHistoryFuels.Any(x => x.CarId == carId)
            ? _context.CarHistoryFuels.Where(x => x.CarId == carId).Max(x => x.Mileage)
            : 0;

        var repairMileage = _context.CarHistoryRepairs.Any(x => x.CarId == carId)
            ? _context.CarHistoryRepairs.Where(x => x.CarId == carId).Max(x => x.Mileage)
            : 0;
        return CarHelper.MileageValidate(fuelMileage, repairMileage, newMileage);
    }
}

