using Microsoft.EntityFrameworkCore;
using Valhalla_v3.Database;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Services.CarHistory;

public interface IMechanicService
{
	Task<int> Create(Mechanic mechanic, int userId);
	Task<Mechanic> Get(int id, int userId);
	Task<List<Mechanic>> Get(int userId);
	Task Update(Mechanic mechanic, int userId);
	Task Delete(int id, int userId);
}

public class MechanicService : IMechanicService
{
    private readonly ValhallaContext _context;

    public MechanicService(ValhallaContext context)
    {
        _context = context;
    }

    public async Task<int> Create(Mechanic mechanic, int userId)
    {
        if (mechanic == null)
            throw new ArgumentNullException(nameof(mechanic), "Mechanic object cannot be null.");

        if (mechanic.Id != 0)
            throw new ArgumentException("Mechanic ID must be 0 for a new entry.");
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        mechanic.DateTimeAdd = DateTime.Now;
        mechanic.DateTimeModify = DateTime.Now;
        mechanic.OperatorCreateId = userId;
        mechanic.OperatorModifyId = userId;

        await _context.Mechanics.AddAsync(mechanic);
        await _context.SaveChangesAsync();

        return mechanic.Id;
    }

    public async Task Delete(int id, int userId)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        if (_context.CarHistoryRepairs.Any(x => x.MechanicId == id))
            throw new ArgumentException("Nie można usunąć mechanika do którego są podpięte naprawy");
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        var mechanic = await _context.Mechanics.FirstOrDefaultAsync(x => x.Id == id && x.OperatorModifyId == userId);

        if (mechanic == null)
            throw new KeyNotFoundException($"Mechanic with ID {id} not found.");

        _context.Mechanics.Remove(mechanic);
        await _context.SaveChangesAsync();
    }

    public async Task<Mechanic> Get(int id, int userId)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        var mechanic = await _context.Mechanics
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .FirstOrDefaultAsync(x => x.Id == id && x.OperatorModifyId == userId);

        if (mechanic == null)
            throw new KeyNotFoundException($"Mechanic with ID {id} not found.");

        return mechanic;
    }

    public async Task<List<Mechanic>> Get(int userId)
    {
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        var mechanicList = await _context.Mechanics
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .Where(x => x.Activ && x.OperatorModifyId == userId)
            .ToListAsync();

        return mechanicList ?? new List<Mechanic>();
    }

    public async Task Update(Mechanic mechanic, int userId)
    {
        if (mechanic == null)
            throw new ArgumentNullException(nameof(mechanic), "Mechanic object cannot be null.");

        if (mechanic.Id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        var existingMechanic = await _context.Mechanics.FirstOrDefaultAsync(x => x.Id == mechanic.Id && x.OperatorModifyId == userId);

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
        existingMechanic.OperatorModifyId = userId;

        await _context.SaveChangesAsync();
    }
}

