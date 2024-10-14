using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Web.Http;
using Valhalla_v3.Database;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Services.CarHistory;

public interface ICarHistoryFuelService
{
	public Task<int> Create(CarHistoryFuel fuel);
	public CarHistoryFuel Get(int id);
	public List<CarHistoryFuel> Get();
	public Task Update(CarHistoryFuel fuel);
	public Task Delete(int id);
}

public class CarHistoryFuelService: ICarHistoryFuelService
{
	private readonly ValhallaComtext _context;

	public CarHistoryFuelService(ValhallaComtext context)
	{
		_context = context;
	}

	public async Task<int> Create(CarHistoryFuel fuel)
	{
		if (fuel.Id != 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
		fuel.DateTimeAdd = DateTime.Now;
		fuel.DateTimeModify = DateTime.Now;
		_context.AddAsync(fuel);
		try
		{
			await _context.SaveChangesAsync();
		}
		catch(Exception ex)
		{
			throw ex;
		}
		return fuel.Id;
	}

	public async Task Delete(int id)
	{
		if (id == 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var fuel = _context.CarHistoryFuels.First(x => x.Id == id);

		if (fuel == null)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));

		_context.CarHistoryFuels.Remove(fuel);
		await _context.SaveChangesAsync();
	}

	public CarHistoryFuel Get(int id)
	{
		if (id == 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var fuel = Get().Find(x => x.Id == id);

		return fuel;
	}

	public List<CarHistoryFuel> Get()
	{
		var FuelList = _context.CarHistoryFuels
			.Include(x => x.OperatorCreate)
			.Include(x => x.OperatorModify)
			.Include(x => x.GasStation)
			.ToList();

		return FuelList;
	}

	public async Task Update(CarHistoryFuel fuel)
	{
		if (fuel.Id != 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var Oldfuel = _context.CarHistoryFuels.First(x => x.Id == fuel.Id);
		if (Oldfuel == null)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
		Oldfuel.Mileage = fuel.Mileage;
		Oldfuel.Date = fuel.Date;
		Oldfuel.Cost = fuel.Cost;
		Oldfuel.CostPerLitr = fuel.CostPerLitr;
		Oldfuel.GasStation = fuel.GasStation;
		Oldfuel.DateTimeModify = DateTime.Now;
		await _context.SaveChangesAsync();
	}
}
