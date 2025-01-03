using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Web.Http;
using Valhalla_v3.Database;
using Valhalla_v3.Shared.CarHistory;
using Valhalla_v3.Shared.ToDo;

namespace Valhalla_v3.Services.ToDo;

public interface IJobService
{
	Task<int> Create(Job job, int userId);
	Task<Job> Get(int id, int userId);
	Task<List<Job>> Get(bool NoDoneJobs, int ProjectId, int userId);
	Task Update(Job job, int userId);
	Task Delete(int id, int userId);
    Task ChangeTerm(int id, DateTime term, int userId);
}

public class JobService : IJobService
{
    private readonly ValhallaContext _context;

    public JobService(ValhallaContext context)
    {
        _context = context;
    }

    public async Task ChangeTerm(int id, DateTime term, int userId)
    {
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        if (id == 0)
            throw new ArgumentException("Job ID must be 0 for a new entry.");
        var job = await _context.Job.FirstOrDefaultAsync(x => x.Id == id && x.OperatorModifyId == userId);
        
        if (job == null)
            throw new ArgumentNullException("Job object cannot be null.");

        job.Term = term;
        job.DateTimeModify = DateTime.Now;
        job.OperatorModifyId = userId;
        await _context.SaveChangesAsync();
    }

    public async Task<int> Create(Job job, int userId)
    {
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        if (job == null)
            throw new ArgumentNullException(nameof(job), "Job object cannot be null.");

        if (job.Id != 0)
            throw new ArgumentException("Job ID must be 0 for a new entry.");

        job.DateTimeAdd = DateTime.Now;
        job.DateTimeModify = DateTime.Now;
        job.OperatorModifyId = userId;
        job.OperatorCreateId = userId;
        await _context.Job.AddAsync(job);
        await _context.SaveChangesAsync();

        return job.Id;
    }

    public async Task Delete(int id, int userId)
    {
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var job = await _context.Job.FirstOrDefaultAsync(x => x.Id == id && x.OperatorModifyId == userId);

        if (job == null)
            throw new KeyNotFoundException($"Job with ID {id} not found.");

        _context.Job.Remove(job);
        await _context.SaveChangesAsync();
    }

    public async Task<Job> Get(int id, int userId)
    {
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var job = await _context.Job
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .Include(x => x.Comments)
            .FirstOrDefaultAsync(x => x.Id == id && x.OperatorModifyId == userId);

        if (job == null)
            throw new KeyNotFoundException($"Job with ID {id} not found.");

        return job;
    }

    public async Task<List<Job>> Get(bool NoDoneJobs, int ProjectId, int userId)
    {
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        var jobs = _context.Job
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .Where(x => x.Term <= DateTime.Now && x.OperatorModifyId == userId);

        if (ProjectId != 0)
        {
            jobs = jobs.Where(x => x.ProjectId == ProjectId);
        }

        if (NoDoneJobs)
        {
            jobs = jobs.Where(x => !x.IsCompleted);
        }

        List<Job> jobsList = await jobs.ToListAsync();

        return jobsList ?? new List<Job>();
    }

    public async Task Update(Job job, int userId)
    {
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        if (job == null)
            throw new ArgumentNullException(nameof(job), "Job object cannot be null.");

        if (job.Id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var existingJob = await _context.Job.FirstOrDefaultAsync(x => x.Id == job.Id && x.OperatorModifyId == userId);

        if (existingJob == null)
            throw new KeyNotFoundException($"Job with ID {job.Id} not found.");

        existingJob.Description = job.Description;
        existingJob.Name = job.Name;
        existingJob.IsCompleted = job.IsCompleted;
        existingJob.Term = job.Term;
        existingJob.ProjectId = job.ProjectId;
        existingJob.DateTimeModify = DateTime.Now;
        existingJob.Comments = job.Comments;
        existingJob.OperatorModifyId = userId;
        await _context.SaveChangesAsync();
    }
}

