using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Web.Http;
using Valhalla_v3.Database;
using Valhalla_v3.Shared.CarHistory;
using static System.Collections.Specialized.BitVector32;

namespace Valhalla_v3.Services.CarHistory;

public interface IMechanicService
{
	public Task<int> Create(Mechanic mechanic);
	public Mechanic Get(int id);
	public List<Mechanic> Get();
	public Task Update(Mechanic mechanic);
	public Task Delete(int id);
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
		if (mechanic.Id != 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
		mechanic.DateTimeAdd = DateTime.Now;
		mechanic.DateTimeModify = DateTime.Now;
		_context.AddAsync(mechanic);
		await _context.SaveChangesAsync();
		return mechanic.Id;
	}

	public async Task Delete(int id)
	{
		if (id == 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var mechanic = _context.Mechanics.First(x => x.Id == id);

		if(mechanic == null)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));

		_context.Mechanics.Remove(mechanic);
		await _context.SaveChangesAsync();
	}

	public Mechanic Get(int id)
	{
		if (id == 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var mechanic = Get().Find(x => x.Id == id);

		return mechanic;
	}

	public List<Mechanic> Get()
	{
		var mechanicList = _context.Mechanics
			.Include(x => x.OperatorCreate)
			.Include(x => x.OperatorModify)
			.ToList();

		return mechanicList;
	}

	public async Task Update(Mechanic mechanic)
	{
		if (mechanic.Id != 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var OldMechanic = _context.Mechanics.First(x => x.Id == mechanic.Id);
		if (OldMechanic == null)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		OldMechanic.Street = mechanic.Street;
		OldMechanic.StreetNumber = mechanic.StreetNumber;
		OldMechanic.PostalCode = mechanic.PostalCode;
		OldMechanic.City = mechanic.City;
		OldMechanic.Country = mechanic.Country;
		OldMechanic.Phone1 = mechanic.Phone1;
		OldMechanic.Phone2 = mechanic.Phone2;
		OldMechanic.DateTimeModify = DateTime.Now;
		await _context.SaveChangesAsync();
	}
}
