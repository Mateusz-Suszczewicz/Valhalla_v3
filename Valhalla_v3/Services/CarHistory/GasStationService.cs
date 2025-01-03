using Microsoft.EntityFrameworkCore;
using Valhalla_v3.Database;
using Valhalla_v3.Migrations;
using Valhalla_v3.Shared.CarHistory;
using Valhalla_v3.Shared.ToDo;

namespace Valhalla_v3.Services.CarHistory;

public interface IGasStationService
{
	Task<int> Create(GasStation station, int userId);
	Task<GasStation> Get(int id, int userId);
	Task<List<GasStation>> Get(int userId);
	Task Update(GasStation station, int userId);
	Task Delete(int id, int userId);
}

public class GasStationService : IGasStationService
{
    private readonly ValhallaContext _context;

    public GasStationService(ValhallaContext context)
    {
        _context = context;
    }

    public async Task<int> Create(GasStation station, int userId)
    {
        if (station == null)
            throw new ArgumentNullException(nameof(station), "Station object cannot be null.");

        if (station.Id != 0)
            throw new ArgumentException("Station ID must be 0 for a new entry.");
       
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        
        station.DateTimeAdd = DateTime.Now;
        station.DateTimeModify = DateTime.Now;
        station.OperatorModifyId = userId;
        station.OperatorCreateId = userId;
        await _context.GasStations.AddAsync(station);
        await _context.SaveChangesAsync();

        return station.Id;
    }

    public async Task Delete(int id, int userId)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var station = await _context.GasStations.FirstOrDefaultAsync(x => x.Id == id && x.OperatorModifyId == userId);

        if (station == null)
            throw new KeyNotFoundException($"Gas station with ID {id} not found.");

        if (_context.CarHistoryFuels.Any(x => x.GasStationId == station.Id))
            throw new ArgumentException("Nie można usunąć stacji do której podłączone jest tankowanie");
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        _context.GasStations.Remove(station);
        await _context.SaveChangesAsync();
    }

    public async Task<GasStation> Get(int id, int userId)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        var station = await _context.GasStations
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .FirstOrDefaultAsync(x => x.Id == id && x.OperatorModifyId == userId);

        if (station == null)
            throw new KeyNotFoundException($"Gas station with ID {id} not found.");

        return station;
    }

    public async Task<List<GasStation>> Get(int userId)
    {
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        var stationList = await _context.GasStations
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .Where(x => x.Activ && x.OperatorModifyId == userId)
            .ToListAsync();

        return stationList ?? new List<GasStation>();
    }

    public async Task Update(GasStation station, int userId)
    {
        if (station == null)
            throw new ArgumentNullException(nameof(station), "Station object cannot be null.");

        if (station.Id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        var existingStation = await _context.GasStations.FirstOrDefaultAsync(x => x.Id == station.Id && x.OperatorModifyId == userId);

        if (existingStation == null)
            throw new KeyNotFoundException($"Gas station with ID {station.Id} not found.");

        existingStation.Street = station.Street;
        existingStation.StreetNumber = station.StreetNumber;
        existingStation.PostalCode = station.PostalCode;
        existingStation.City = station.City;
        existingStation.Country = station.Country;
        existingStation.Phone1 = station.Phone1;
        existingStation.Phone2 = station.Phone2;
        existingStation.DateTimeModify = DateTime.Now;
        existingStation.OperatorModifyId = userId;

        await _context.SaveChangesAsync();
    }
}

