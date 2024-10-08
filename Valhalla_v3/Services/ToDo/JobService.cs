﻿using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Web.Http;
using Valhalla_v3.Database;
using Valhalla_v3.Shared.CarHistory;
using Valhalla_v3.Shared.ToDo;

namespace Valhalla_v3.Services.ToDo;

public interface IJobService
{
	public Task<int> Create(Job job);
	public Job Get(int id);
	public List<Job> Get();
	public Task Update(Job job);
	public Task Delete(int id);
}

public class JobService : IJobService
{
	private readonly ValhallaComtext _context;

	public JobService(ValhallaComtext context) 
	{
		_context = context;
	}

	public async Task<int> Create(Job job)
	{
		if (job.Id != 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
		job.DateTimeAdd = DateTime.Now;
		job.DateTimeModify = DateTime.Now;
		_context.AddAsync(job);
		await _context.SaveChangesAsync();
		return job.Id;
	}

	public async Task Delete(int id)
	{
		if (id == 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var job = _context.Job.First(x => x.Id == id);

		if(job == null)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));

		_context.Job.Remove(job);
		await _context.SaveChangesAsync();
	}

	public Job Get(int id)
	{
		if (id == 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var job = Get().Find(x => x.Id == id);

		return job;
	}

	public List<Job> Get()
	{
		var jobList = _context.Job
			.Include(x => x.OperatorCreate)
			.Include(x => x.OperatorModify)
			.ToList();

		return jobList;
	}

	public async Task Update(Job job)
	{
		if (job.Id != 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var OldJob = _context.Job.First(x => x.Id == job.Id);
		if (OldJob == null)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
		OldJob.Description = job.Description;
		OldJob.Name = job.Name;
		OldJob.IsCompleted = job.IsCompleted;
		OldJob.Term = job.Term;
		OldJob.Project = job.Project;
		OldJob.DateTimeModify = DateTime.Now;
		await _context.SaveChangesAsync();
	}
}
