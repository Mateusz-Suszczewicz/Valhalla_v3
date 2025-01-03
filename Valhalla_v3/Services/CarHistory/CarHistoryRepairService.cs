using Microsoft.EntityFrameworkCore;
using Valhalla_v3.Database;
using Valhalla_v3.Migrations;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Services.CarHistory;

public interface ICarHistoryRepairService
{
	Task<int> Create(CarHistoryRepair Repair, int userId);
	Task<CarHistoryRepair> Get(int id, int userId);
	Task<List<CarHistoryRepair>> Get(int userId);
	Task Update(CarHistoryRepair Repair, int userId);
	Task Delete(int id, int userId);
}

public class CarHistoryRepairService : ICarHistoryRepairService
{
    private readonly ValhallaContext _context;

    public CarHistoryRepairService(ValhallaContext context)
    {
        _context = context;
    }

    public async Task<int> Create(CarHistoryRepair repair, int userId)
    {
        if (repair == null)
            throw new ArgumentNullException(nameof(repair), "Repair object cannot be null.");

        if (repair.Id != 0)
            throw new ArgumentException("Repair ID must be 0 for a new entry.");
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        if (!validMileage(repair.Mileage))
            throw new ArgumentException("Mileage must be greater than saved mileage.");
        
        repair.DateTimeAdd = DateTime.Now;
        repair.DateTimeModify = DateTime.Now;
        repair.OperatorModifyId = userId;
        repair.OperatorCreateId = userId;
        await _context.CarHistoryRepairs.AddAsync(repair);
        await _context.SaveChangesAsync();

        return repair.Id;
    }

    public async Task Delete(int id, int userId)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        var repair = await _context.CarHistoryRepairs.FirstOrDefaultAsync(x => x.Id == id && x.OperatorModifyId == userId);

        if (repair == null)
            throw new KeyNotFoundException($"Repair with ID {id} not found.");

        _context.CarHistoryRepairs.Remove(repair);
        await _context.SaveChangesAsync();
    }

    public async Task<CarHistoryRepair> Get(int id, int userId)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        var repair = await _context.CarHistoryRepairs
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .Include(x => x.Mechanic)
            .FirstOrDefaultAsync(x => x.Id == id && x.OperatorModifyId == userId);

        if (repair == null)
            throw new KeyNotFoundException($"Repair with ID {id} not found.");

        return repair;
    }

    public async Task<List<CarHistoryRepair>> Get(int userId)
    {
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        var repairList = await _context.CarHistoryRepairs
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .Include(x => x.Mechanic)
            .Where(x => x.OperatorModifyId == userId)
            .ToListAsync();

        return repairList ?? new List<CarHistoryRepair>();
    }

    public async Task Update(CarHistoryRepair repair, int userId)
    {
        if (repair == null)
            throw new ArgumentNullException(nameof(repair), "Repair object cannot be null.");
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
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
        existingRepair.OperatorModifyId = userId;
        
        await _context.SaveChangesAsync();
    }

    private bool validMileage(int newMileage)
    {
        var fuelMileage = _context.CarHistoryFuels.Max(x => x.Mileage);
        var repairMileage = _context.CarHistoryRepairs.Max(x => x.Mileage);
        return CarHelper.MileageValidate(fuelMileage, repairMileage, newMileage);
    }
}

