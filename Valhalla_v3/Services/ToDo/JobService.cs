using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Web.Http;
using Valhalla_v3.Database;
using Valhalla_v3.Shared.CarHistory;
using Valhalla_v3.Shared.ToDo;

namespace Valhalla_v3.Services.ToDo;

public interface IJobService
{
	Task<int> Create(Job job);
	Task<Job> Get(int id);
	Task<List<Job>> Get();
	Task Update(Job job);
	Task Delete(int id);
}

public class JobService : IJobService
{
    private readonly ValhallaContext _context;

    public JobService(ValhallaContext context)
    {
        _context = context;
    }

    public async Task<int> Create(Job job)
    {
        if (job == null)
            throw new ArgumentNullException(nameof(job), "Job object cannot be null.");

        if (job.Id != 0)
            throw new ArgumentException("Job ID must be 0 for a new entry.");

        job.DateTimeAdd = DateTime.Now;
        job.DateTimeModify = DateTime.Now;

        await _context.Job.AddAsync(job);
        await _context.SaveChangesAsync();

        return job.Id;
    }

    public async Task Delete(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var job = await _context.Job.FirstOrDefaultAsync(x => x.Id == id);

        if (job == null)
            throw new KeyNotFoundException($"Job with ID {id} not found.");

        _context.Job.Remove(job);
        await _context.SaveChangesAsync();
    }

    public async Task<Job> Get(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var job = await _context.Job
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .Include(x => x.Comments)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (job == null)
            throw new KeyNotFoundException($"Job with ID {id} not found.");

        return job;
    }

    public async Task<List<Job>> Get()
    {
        var jobList = await _context.Job
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .ToListAsync();

        return jobList ?? new List<Job>();
    }

    public async Task Update(Job job)
    {
        if (job == null)
            throw new ArgumentNullException(nameof(job), "Job object cannot be null.");

        if (job.Id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var existingJob = await _context.Job.FirstOrDefaultAsync(x => x.Id == job.Id);

        if (existingJob == null)
            throw new KeyNotFoundException($"Job with ID {job.Id} not found.");

        existingJob.Description = job.Description;
        existingJob.Name = job.Name;
        existingJob.IsCompleted = job.IsCompleted;
        existingJob.Term = job.Term;
        existingJob.ProjectId = job.ProjectId;
        existingJob.DateTimeModify = DateTime.Now;
        existingJob.Comments = job.Comments;

        await _context.SaveChangesAsync();
    }
}

