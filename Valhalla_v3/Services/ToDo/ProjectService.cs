using Microsoft.EntityFrameworkCore;
using Valhalla_v3.Database;
using Valhalla_v3.Shared.ToDo;

namespace Valhalla_v3.Services.ToDo;

public interface IProjectService
{
	Task<int> Create(Project project);
	Task<Project> Get(int id);
	Task<List<Project>> Get();
	Task Update(Project project);
	Task Delete(int id);
}

public class ProjectService : IProjectService
{
    private readonly ValhallaContext _context;

    public ProjectService(ValhallaContext context)
    {
        _context = context;
    }

    public async Task<int> Create(Project project)
    {
        if (project == null)
            throw new ArgumentNullException(nameof(project), "Project object cannot be null.");

        if (project.Id != 0)
            throw new ArgumentException("Project ID must be 0 for a new entry.");

        project.DateTimeAdd = DateTime.Now;
        project.DateTimeModify = DateTime.Now;

        await _context.Project.AddAsync(project);
        await _context.SaveChangesAsync();

        return project.Id;
    }

    public async Task Delete(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var project = await _context.Project.FirstOrDefaultAsync(x => x.Id == id);

        if (project == null)
            throw new KeyNotFoundException($"Project with ID {id} not found.");

        _context.Project.Remove(project);
        await _context.SaveChangesAsync();
    }

    public async Task<Project> Get(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var project = await _context.Project
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .Include(x => x.Tasks)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (project == null)
            throw new KeyNotFoundException($"Project with ID {id} not found.");

        return project;
    }

    public async Task<List<Project>> Get()
    {
        var projectList = await _context.Project
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .Include(x => x.Tasks)
            .ToListAsync();

        return projectList ?? new List<Project>();
    }

    public async Task Update(Project project)
    {
        if (project == null)
            throw new ArgumentNullException(nameof(project), "Project object cannot be null.");

        if (project.Id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var existingProject = await _context.Project.FirstOrDefaultAsync(x => x.Id == project.Id);

        if (existingProject == null)
            throw new KeyNotFoundException($"Project with ID {project.Id} not found.");

        existingProject.Tasks = project.Tasks;
        existingProject.DateTimeModify = DateTime.Now;

        await _context.SaveChangesAsync();
    }
}

