﻿using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Web.Http;
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

public class OperatorService(ValhallaComtext context) : IOperatorService
{
    private readonly ValhallaComtext _context = context;

    public async Task<int> Create(Operator oper)
    {
        if (oper.Id != 0)
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
		oper.DateTimeAdd = DateTime.Now;
		oper.DateTimeModify = DateTime.Now;
        await _context.AddAsync(oper);
        await _context.SaveChangesAsync();
        return oper.Id;
    }

    public async Task Delete(int id)
    {
        if (id == 0)
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

        var oper = _context.Operator.First(x => x.Id == id) ?? throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
        _context.Operator.Remove(oper);
        await _context.SaveChangesAsync();
    }

    public async Task<Operator> Get(int id)
    {
        if (id == 0)
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

        var oper = await _context.Operator
            .FirstOrDefaultAsync(x => x.Id == id);
        return oper;
    }

    public async Task<List<Operator>> Get()
    {
        var operList = await _context.Operator
            .ToListAsync();

        return  operList;
    }

    public async Task Update(Operator oper)
    {
        if (oper.Id != 0)
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

        var Oldoper = _context.Operator.First(x => x.Id == oper.Id) ?? throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
        Oldoper.Name = oper.Name;
		Oldoper.Password = oper.Password;
		Oldoper.DateTimeModify = DateTime.Now;
        await _context.SaveChangesAsync();
    }
}
