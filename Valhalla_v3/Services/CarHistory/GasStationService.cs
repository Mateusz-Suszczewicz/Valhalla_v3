using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Web.Http;
using Valhalla_v3.Database;
using Valhalla_v3.Shared.CarHistory;
using static System.Collections.Specialized.BitVector32;

namespace Valhalla_v3.Services.CarHistory;

public interface IGasStationService
{
	public Task<int> Create(GasStation station);
	public GasStation Get(int id);
	public List<GasStation> Get();
	public Task Update(GasStation station);
	public Task Delete(int id);
}

public class GasStationService : IGasStationService
{
	private readonly ValhallaComtext _context;

	public GasStationService(ValhallaComtext context) 
	{
		_context = context;
	}

	public async Task<int> Create(GasStation station)
	{ 
		if (station.Id != 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
		station.DateTimeAdd = DateTime.Now;
		station.DateTimeModify = DateTime.Now;
		_context.AddAsync(station);
		await _context.SaveChangesAsync();
		return station.Id;
	}

	public async Task Delete(int id)
	{
		if (id == 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var station = _context.GasStations.First(x => x.Id == id);

		if(station == null)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));

		_context.GasStations.Remove(station);
		await _context.SaveChangesAsync();
	}

	public GasStation Get(int id)
	{
		if (id == 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var station = Get().Find(x => x.Id == id);

		return station;
	}

	public List<GasStation> Get()
	{
		var StationList = _context.GasStations
			.Include(x => x.OperatorCreate)
			.Include(x => x.OperatorModify)
			.ToList();

		return StationList;
	}

	public async Task Update(GasStation station)
	{
		if (station.Id != 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var Oldstation = _context.GasStations.First(x => x.Id == station.Id);
		if (Oldstation == null)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
		Oldstation.Street = station.Street;
		Oldstation.StreetNumber = station.StreetNumber;
		Oldstation.PostalCode = station.PostalCode;
		Oldstation.City = station.City;
		Oldstation.Country = station.Country;
		Oldstation.Phone1 = station.Phone1;
		Oldstation.Phone2 = station.Phone2;
		Oldstation.DateTimeModify = DateTime.Now;
		await _context.SaveChangesAsync();
	}
}
