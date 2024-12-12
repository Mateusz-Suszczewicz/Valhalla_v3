using Microsoft.EntityFrameworkCore;
using Valhalla_v3.Database;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Services.CarHistory;

public interface IMechanicService
{
	Task<int> Create(Mechanic mechanic);
	Task<Mechanic> Get(int id);
	Task<List<Mechanic>> Get();
	Task Update(Mechanic mechanic);
	Task Delete(int id);
}

public class MechanicService : IMechanicService
{
    private readonly ValhallaComtext _context;

    public MechanicService(ValhallaComtext context)
    {
        _context = context;
    }

    public async Task<int> Create(Mechanic mechanic)
    {
        if (mechanic == null)
            throw new ArgumentNullException(nameof(mechanic), "Mechanic object cannot be null.");

        if (mechanic.Id != 0)
            throw new ArgumentException("Mechanic ID must be 0 for a new entry.");

        mechanic.DateTimeAdd = DateTime.Now;
        mechanic.DateTimeModify = DateTime.Now;

        await _context.Mechanics.AddAsync(mechanic);
        await _context.SaveChangesAsync();

        return mechanic.Id;
    }

    public async Task Delete(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var mechanic = await _context.Mechanics.FirstOrDefaultAsync(x => x.Id == id);

        if (mechanic == null)
            throw new KeyNotFoundException($"Mechanic with ID {id} not found.");

        _context.Mechanics.Remove(mechanic);
        await _context.SaveChangesAsync();
    }

    public async Task<Mechanic> Get(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var mechanic = await _context.Mechanics
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (mechanic == null)
            throw new KeyNotFoundException($"Mechanic with ID {id} not found.");

        return mechanic;
    }

    public async Task<List<Mechanic>> Get()
    {
        var mechanicList = await _context.Mechanics
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .ToListAsync();

        return mechanicList ?? new List<Mechanic>();
    }

    public async Task Update(Mechanic mechanic)
    {
        if (mechanic == null)
            throw new ArgumentNullException(nameof(mechanic), "Mechanic object cannot be null.");

        if (mechanic.Id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var existingMechanic = await _context.Mechanics.FirstOrDefaultAsync(x => x.Id == mechanic.Id);

        if (existingMechanic == null)
            throw new KeyNotFoundException($"Mechanic with ID {mechanic.Id} not found.");

        existingMechanic.Street = mechanic.Street;
        existingMechanic.StreetNumber = mechanic.StreetNumber;
        existingMechanic.PostalCode = mechanic.PostalCode;
        existingMechanic.City = mechanic.City;
        existingMechanic.Country = mechanic.Country;
        existingMechanic.Phone1 = mechanic.Phone1;
        existingMechanic.Phone2 = mechanic.Phone2;
        existingMechanic.DateTimeModify = DateTime.Now;

        await _context.SaveChangesAsync();
    }
}

