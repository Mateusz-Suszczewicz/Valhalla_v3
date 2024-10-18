using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Web.Http;
using Valhalla_v3.Database;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Services.CarHistory;

public interface ICarHistoryRepairService
{
	public Task<int> Create(CarHistoryRepair Repair);
	public CarHistoryRepair Get(int id);
	public List<CarHistoryRepair> Get();
	public Task Update(CarHistoryRepair Repair);
	public Task Delete(int id);
}

public class CarHistoryRepairService : ICarHistoryRepairService
{
	private readonly ValhallaComtext _context;

	public CarHistoryRepairService(ValhallaComtext context)
	{
		_context = context;
	}

	public async Task<int> Create(CarHistoryRepair Repair)
	{
		if (Repair.Id != 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
		try
		{
			Repair.DateTimeAdd = DateTime.Now;
			Repair.DateTimeModify = DateTime.Now;
			_context.AddAsync(Repair);
			await _context.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			throw ex;
		}
		return Repair.Id;
	}

	public async Task Delete(int id)
	{
		if (id == 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var repair = _context.CarHistoryRepairs.First(x => x.Id == id);

		if (repair == null)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));

		_context.CarHistoryRepairs.Remove(repair);
		await _context.SaveChangesAsync();
	}

	public CarHistoryRepair Get(int id)
	{
		if (id == 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var repair = Get().Find(x => x.Id == id);

		return repair;
	}

	public List<CarHistoryRepair> Get()
	{
		var RepairList = _context.CarHistoryRepairs
			.Include(x => x.OperatorCreate)
			.Include(x => x.OperatorModify)
			.Include(x => x.Mechanic)
			.ToList();

		return RepairList;
	}

	public async Task Update(CarHistoryRepair repair)
	{
		if (repair.Id != 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var OldRepair = _context.CarHistoryRepairs.First(x => x.Id == repair.Id);
		if (OldRepair == null)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
		OldRepair.Mileage = repair.Mileage;
		OldRepair.Date = repair.Date;
		OldRepair.Cost = repair.Cost;
		OldRepair.Description = repair.Description;
		OldRepair.Mechanic = repair.Mechanic;
		OldRepair.DateTimeModify = DateTime.Now;
		await _context.SaveChangesAsync();
	}
}
