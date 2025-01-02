using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;
using System.Web.Http;
using Valhalla_v3.Database;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Services.CarHistory;

public interface ICarService
{
	public Task<int> Create(Car car, int userId);
	public Task<Car> Get(int Carid, int userId);
	public Task<List<Car>> Get(int userId);
	public Task Update(Car car, int userId);
	public Task Delete(int id, int userId);
}

public class CarService : ICarService
{
	private readonly ValhallaContext _context;

	public CarService(ValhallaContext context) 
	{
		_context = context;
	}

    public async Task<int> Create(Car car, int userId)
    {
        if (car == null)
            throw new ArgumentException("Car object cannot be null.");

        if (car.Id != 0)
            throw new ArgumentException("Cannot create a car with an existing ID.");

		if (userId == 0)
			throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        try
        {
            car.DateTimeAdd = DateTime.Now;
            car.DateTimeModify = DateTime.Now;
			car.OperatorModifyId = userId;
			car.OperatorCreateId = userId;
            await _context.AddAsync(car);
            await _context.SaveChangesAsync();
            return car.Id;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
            throw new InvalidOperationException("An error occurred while saving the car to the database.", ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            throw new InvalidOperationException("An unexpected error occurred.", ex);
        }
    }

    public async Task Delete(int id, int userId)
	{
		if (id == 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        var car = await _context.Car
                .Include(x => x.Fuels)
            .ThenInclude(y => y.GasStation)
            .Include(x => x.CarHistoryRepair)
            .ThenInclude(z => z.Mechanic)
            .FirstAsync(x => x.Id == id && x.OperatorModifyId == userId);

		if(car == null)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));

		try
		{
			_context.Remove(car);
			await _context.SaveChangesAsync();
		}
		catch(Exception ex)
		{
			throw ex;
		}
	}

	public async Task<Car> Get(int Carid, int userId)
	{
		if (Carid == 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        var car = await _context.Car
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
			.Include(x => x.Fuels)
			.ThenInclude(y => y.GasStation)
            .Include(x => x.CarHistoryRepair)
            .ThenInclude(z => z.Mechanic)
            .FirstOrDefaultAsync(x => x.Id == Carid && x.OperatorModifyId == userId);
        return car;
	}
	
	public async Task<List<Car>> Get(int userId)
	{
		try
		{
            if (userId == 0)
                throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
            var CarList = await _context.Car
				.Include(x => x.OperatorCreate)
				.Include(x => x.OperatorModify)
				.Where(x => x.OperatorModifyId == userId)
				.ToListAsync();

			return CarList;
		}
		catch(Exception ex)
		{
			return new List<Car>();
		}
	}

	public async Task Update(Car car, int userId)
	{
		if (car.Id == 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        var OldCar = _context.Car.First(x => x.Id == car.Id);
		if (OldCar == null)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
		OldCar.Year = car.Year;
		OldCar.VIN = car.VIN;
		OldCar.Brand = car.Brand;
		OldCar.Model = car.Model;
		OldCar.EngineCC = car.EngineCC;
		OldCar.DateTimeModify = DateTime.Now;
		OldCar.InsuranceCost = car.InsuranceCost;
		OldCar.InsuranceDate = car.InsuranceDate;
		OldCar.SurveyDate = car.SurveyDate;
		OldCar.SurveyCost = car.SurveyCost;
		OldCar.OperatorModifyId = userId;
        await _context.SaveChangesAsync();
    }
}
