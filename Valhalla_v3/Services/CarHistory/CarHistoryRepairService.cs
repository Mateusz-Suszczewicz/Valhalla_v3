using Microsoft.EntityFrameworkCore;
using Valhalla_v3.Database;
using Valhalla_v3.Migrations;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Services.CarHistory;

public interface ICarHistoryRepairService
{
	Task<int> Create(CarHistoryRepair Repair);
	Task<CarHistoryRepair> Get(int id);
	Task<List<CarHistoryRepair>> Get();
	Task Update(CarHistoryRepair Repair);
	Task Delete(int id);
}

public class CarHistoryRepairService : ICarHistoryRepairService
{
    private readonly ValhallaContext _context;

    public CarHistoryRepairService(ValhallaContext context)
    {
        _context = context;
    }

    public async Task<int> Create(CarHistoryRepair repair)
    {
        if (repair == null)
            throw new ArgumentNullException(nameof(repair), "Repair object cannot be null.");

        if (repair.Id != 0)
            throw new ArgumentException("Repair ID must be 0 for a new entry.");
        
        if (!validMileage(repair.Mileage))
            throw new ArgumentException("Mileage must be greater than saved mileage.");
        
        repair.DateTimeAdd = DateTime.Now;
        repair.DateTimeModify = DateTime.Now;

        await _context.CarHistoryRepairs.AddAsync(repair);
        await _context.SaveChangesAsync();

        return repair.Id;
    }

    public async Task Delete(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var repair = await _context.CarHistoryRepairs.FirstOrDefaultAsync(x => x.Id == id);

        if (repair == null)
            throw new KeyNotFoundException($"Repair with ID {id} not found.");

        _context.CarHistoryRepairs.Remove(repair);
        await _context.SaveChangesAsync();
    }

    public async Task<CarHistoryRepair> Get(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var repair = await _context.CarHistoryRepairs
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .Include(x => x.Mechanic)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (repair == null)
            throw new KeyNotFoundException($"Repair with ID {id} not found.");

        return repair;
    }

    public async Task<List<CarHistoryRepair>> Get()
    {
        var repairList = await _context.CarHistoryRepairs
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .Include(x => x.Mechanic)
            .ToListAsync();

        return repairList ?? new List<CarHistoryRepair>();
    }

    public async Task Update(CarHistoryRepair repair)
    {
        if (repair == null)
            throw new ArgumentNullException(nameof(repair), "Repair object cannot be null.");

        if (repair.Id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");
        
        if (!validMileage(repair.Mileage))
            throw new ArgumentException("Mileage must be greater than saved mileage.");
        
        var existingRepair = await _context.CarHistoryRepairs.FirstOrDefaultAsync(x => x.Id == repair.Id);

        if (existingRepair == null)
            throw new KeyNotFoundException($"Repair with ID {repair.Id} not found.");

        existingRepair.Mileage = repair.Mileage;
        existingRepair.Date = repair.Date;
        existingRepair.Cost = repair.Cost;
        existingRepair.Description = repair.Description;
        existingRepair.Mechanic = repair.Mechanic;
        existingRepair.DateTimeModify = DateTime.Now;

        await _context.SaveChangesAsync();
    }

    private bool validMileage(int newMileage)
    {
        var fuelMileage = _context.CarHistoryFuels.Max(x => x.Mileage);
        var repairMileage = _context.CarHistoryRepairs.Max(x => x.Mileage);
        return CarHelper.MileageValidate(fuelMileage, repairMileage, newMileage);
    }
}

