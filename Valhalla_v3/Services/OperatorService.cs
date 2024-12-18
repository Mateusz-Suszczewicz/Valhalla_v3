using Microsoft.EntityFrameworkCore;
using Valhalla_v3.Database;
using Valhalla_v3.Shared;

namespace Valhalla_v3.Services;

public interface IOperatorService
{
    public Task<int> Create(Operator oper);
    public Task<Operator> Get(int id);
    public Task<List<Operator>> Get();
    public Task Update(Operator oper);
    public Task Delete(int id);
}

public class OperatorService : IOperatorService
{
    private readonly ValhallaContext _context;

    public OperatorService(ValhallaContext context)
    {
        _context = context;
    }

    public async Task<int> Create(Operator oper)
    {
        if (oper == null)
            throw new ArgumentNullException(nameof(oper), "Operator object cannot be null.");

        if (oper.Id != 0)
            throw new ArgumentException("Operator ID must be 0 for a new entry.");

        oper.DateTimeAdd = DateTime.Now;
        oper.DateTimeModify = DateTime.Now;

        await _context.Operator.AddAsync(oper);
        await _context.SaveChangesAsync();

        return oper.Id;
    }

    public async Task Delete(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var oper = await _context.Operator.FirstOrDefaultAsync(x => x.Id == id);

        if (oper == null)
            throw new KeyNotFoundException($"Operator with ID {id} not found.");

        _context.Operator.Remove(oper);
        await _context.SaveChangesAsync();
    }

    public async Task<Operator> Get(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var oper = await _context.Operator.FirstOrDefaultAsync(x => x.Id == id);

        if (oper == null)
            throw new KeyNotFoundException($"Operator with ID {id} not found.");

        return oper;
    }

    public async Task<List<Operator>> Get()
    {
        var operList = await _context.Operator.ToListAsync();

        return operList ?? new List<Operator>();
    }

    public async Task Update(Operator oper)
    {
        if (oper == null)
            throw new ArgumentNullException(nameof(oper), "Operator object cannot be null.");

        if (oper.Id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var existingOper = await _context.Operator.FirstOrDefaultAsync(x => x.Id == oper.Id);

        if (existingOper == null)
            throw new KeyNotFoundException($"Operator with ID {oper.Id} not found.");

        existingOper.Name = oper.Name;
        existingOper.Password = oper.Password;
        existingOper.DateTimeModify = DateTime.Now;

        await _context.SaveChangesAsync();
    }
}
