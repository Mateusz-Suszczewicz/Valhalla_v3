using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Web.Http;
using Valhalla_v3.Database;
using Valhalla_v3.Shared.CarHistory;
using Valhalla_v3.Shared.ToDo;

namespace Valhalla_v3.Services.ToDo;

public interface IProjectService
{
	public Task<int> Create(Project project);
	public Project Get(int id);
	public List<Project> Get();
	public Task Update(Project project);
	public Task Delete(int id);
}

public class ProjectService : IProjectService
{
	private readonly ValhallaComtext _context;

	public ProjectService(ValhallaComtext context) 
	{
		_context = context;
	}

	public async Task<int> Create(Project project)
	{
		if (project.Id != 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
		project.DateTimeAdd = DateTime.Now;
		project.DateTimeModify = DateTime.Now;
		_context.AddAsync(project);
		await _context.SaveChangesAsync();
		return project.Id;
	}

	public async Task Delete(int id)
	{
		if (id == 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var project = _context.Project.First(x => x.Id == id);

		if(project == null)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));

		_context.Project.Remove(project);
		await _context.SaveChangesAsync();
	}

	public Project Get(int id)
	{
		if (id == 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var project = Get().Find(x => x.Id == id);

		return project;
	}

	public List<Project> Get()
	{
		var ProjectList = _context.Project
			.Include(x => x.OperatorCreate)
			.Include(x => x.OperatorModify)
			.Include(x => x.Tasks)
			.ToList();

		return ProjectList;
	}

	public async Task Update(Project project)
	{
		if (project.Id != 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var OldProject = _context.Project.First(x => x.Id == project.Id);
		if (OldProject == null)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
		OldProject.Tasks = project.Tasks;
		OldProject.DateTimeModify = DateTime.Now;
		await _context.SaveChangesAsync();
	}
}
