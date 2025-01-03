using Microsoft.EntityFrameworkCore;
using Valhalla_v3.Database;
using Valhalla_v3.Shared.ToDo;

namespace Valhalla_v3.Services.ToDo;

public interface IProjectService
{
	Task<int> Create(Project project, int userId);
	Task<Project> Get(int id, int userId);
	Task<List<Project>> Get(int userId);
	Task Update(Project project, int userId);
	Task Delete(int id, int userId);
}

public class ProjectService : IProjectService
{
    private readonly ValhallaContext _context;

    public ProjectService(ValhallaContext context)
    {
        _context = context;
    }

    public async Task<int> Create(Project project, int userId)
    {
        if (project == null)
            throw new ArgumentNullException(nameof(project), "Project object cannot be null.");

        if (project.Id != 0)
            throw new ArgumentException("Project ID must be 0 for a new entry.");
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        project.DateTimeAdd = DateTime.Now;
        project.DateTimeModify = DateTime.Now;
        project.OperatorModifyId = userId;
        project.OperatorCreateId = userId;
        await _context.Project.AddAsync(project);
        await _context.SaveChangesAsync();

        return project.Id;
    }

    public async Task Delete(int id, int userId)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        var project = await _context.Project.FirstOrDefaultAsync(x => x.Id == id);

        if (project == null)
            throw new KeyNotFoundException($"Project with ID {id} not found.");
        
        if (_context.Job.Any(x => x.ProjectId == id && x.OperatorModifyId == userId))
            throw new ArgumentException("Nie można usunąć projektu który ma przypisane zadania");
        
        _context.Project.Remove(project);
        await _context.SaveChangesAsync();
    }

    public async Task<Project> Get(int id, int userId)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        var project = await _context.Project
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .Include(x => x.Tasks)
            .FirstOrDefaultAsync(x => x.Id == id && x.OperatorModifyId == userId);

        if (project == null)
            throw new KeyNotFoundException($"Project with ID {id} not found.");

        return project;
    }

    public async Task<List<Project>> Get(int userId)
    {
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        var projectList = await _context.Project
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .Include(x => x.Tasks)
            .Where(x => x.OperatorModifyId == userId)
            .ToListAsync();

        return projectList ?? new List<Project>();
    }

    public async Task Update(Project project, int userId)
    {
        if (project == null)
            throw new ArgumentNullException(nameof(project), "Project object cannot be null.");
        if (userId == 0)
            throw new ArgumentException("Błąd w przekazywanym Id użytkownika");
        if (project.Id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");
        
        if (_context.Job.Any(x => x.ProjectId == project.Id && !x.IsCompleted && x.OperatorModifyId == userId) && !project.Activ)
            throw new ArgumentException("Zakończony projekt nie moze mieć aktywnych zadań.");

        var existingProject = await _context.Project.FirstOrDefaultAsync(x => x.Id == project.Id && x.OperatorModifyId == userId);

        if (existingProject == null)
            throw new KeyNotFoundException($"Project with ID {project.Id} not found.");

        existingProject.Tasks = project.Tasks;
        existingProject.DateTimeModify = DateTime.Now;
        existingProject.OperatorModifyId = userId;
        await _context.SaveChangesAsync();
    }
}

