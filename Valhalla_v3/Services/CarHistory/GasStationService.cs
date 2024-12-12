using Microsoft.EntityFrameworkCore;
using Valhalla_v3.Database;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Services.CarHistory;

public interface IGasStationService
{
	Task<int> Create(GasStation station);
	Task<GasStation> Get(int id);
	Task<List<GasStation>> Get();
	Task Update(GasStation station);
	Task Delete(int id);
}

public class GasStationService : IGasStationService
{
    private readonly ValhallaContext _context;

    public GasStationService(ValhallaContext context)
    {
        _context = context;
    }

    public async Task<int> Create(GasStation station)
    {
        if (station == null)
            throw new ArgumentNullException(nameof(station), "Station object cannot be null.");

        if (station.Id != 0)
            throw new ArgumentException("Station ID must be 0 for a new entry.");

        station.DateTimeAdd = DateTime.Now;
        station.DateTimeModify = DateTime.Now;

        await _context.GasStations.AddAsync(station);
        await _context.SaveChangesAsync();

        return station.Id;
    }

    public async Task Delete(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var station = await _context.GasStations.FirstOrDefaultAsync(x => x.Id == id);

        if (station == null)
            throw new KeyNotFoundException($"Gas station with ID {id} not found.");

        _context.GasStations.Remove(station);
        await _context.SaveChangesAsync();
    }

    public async Task<GasStation> Get(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var station = await _context.GasStations
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (station == null)
            throw new KeyNotFoundException($"Gas station with ID {id} not found.");

        return station;
    }

    public async Task<List<GasStation>> Get()
    {
        var stationList = await _context.GasStations
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .ToListAsync();

        return stationList ?? new List<GasStation>();
    }

    public async Task Update(GasStation station)
    {
        if (station == null)
            throw new ArgumentNullException(nameof(station), "Station object cannot be null.");

        if (station.Id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var existingStation = await _context.GasStations.FirstOrDefaultAsync(x => x.Id == station.Id);

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

        await _context.SaveChangesAsync();
    }
}

