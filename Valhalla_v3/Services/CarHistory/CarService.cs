using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;
using System.Web.Http;
using Valhalla_v3.Database;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Services.CarHistory;

public interface ICarService
{
	public Task<int> Create(Car car);
	public Car Get(int id);
	public List<Car> Get();
	public Task Update(Car car);
	public Task Delete(int id);
}

public class CarService : ICarService
{
	private readonly ValhallaComtext _context;

	public CarService(ValhallaComtext context) 
	{
		_context = context;
	}

	public async Task<int> Create(Car car)
	{
		if (car.Id != 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
		car.DateTimeAdd = DateTime.Now;
		car.DateTimeModify = DateTime.Now;
		_context.AddAsync(car);
		await _context.SaveChangesAsync();
		return car.Id;
	}

	public async Task Delete(int id)
	{
		if (id == 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var car = _context.Car.First(x => x.Id == id);

		if(car == null)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));

		_context.Car.Remove(car);
		await _context.SaveChangesAsync();
	}

	//TODO: pobierać pojedyńczy z bazy nie z metody GET
	public Car Get(int id)
	{
		if (id == 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
		
			var car = Get()
				.FirstOrDefault(x => x.Id == id);
		return car;
	}
	//TODO: Zmienić na async
	public List<Car> Get()
	{
		var CarList = _context.Car
			.Include(x => x.OperatorCreate)
			.Include(x => x.OperatorModify)
            .ToList();

		return CarList;
	}

	public async Task Update(Car car)
	{
		if (car.Id != 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var OldCar = _context.Car.First(x => x.Id == car.Id);
		if (OldCar == null)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
		OldCar.Year = car.Year;
		OldCar.VIN = car.VIN;
		OldCar.Brand = car.Brand;
		OldCar.Model = car.Model;
		OldCar.EngineCC = car.EngineCC;
		OldCar.DateTimeModify = DateTime.Now;
		await _context.SaveChangesAsync();
	}
}
