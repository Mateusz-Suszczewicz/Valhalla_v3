using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Web.Http;
using Valhalla_v3.Database;
using Valhalla_v3.Shared.CarHistory;
using Valhalla_v3.Shared.ToDo;

namespace Valhalla_v3.Services.ToDo;

public interface ICommentService
{
	public Task<int> Create(Comment comment);
	public Comment Get(int id);
	public List<Comment> Get();
	public Task Update(Comment comment);
	public Task Delete(int id);
}

public class CommentService : ICommentService
{
	private readonly ValhallaComtext _context;

	public CommentService(ValhallaComtext context) 
	{
		_context = context;
	}

	public async Task<int> Create(Comment comment)
	{
		if (comment.Id != 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
		comment.DateTimeAdd = DateTime.Now;
		comment.DateTimeModify = DateTime.Now;
		_context.AddAsync(comment);
		await _context.SaveChangesAsync();
		return comment.Id;
	}

	public async Task Delete(int id)
	{
		if (id == 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var comment = _context.Comment.First(x => x.Id == id);

		if(comment == null)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));

		_context.Comment.Remove(comment);
		await _context.SaveChangesAsync();
	}

	public Comment Get(int id)
	{
		if (id == 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var comment = Get().Find(x => x.Id == id);

		return comment;
	}

	public List<Comment> Get()
	{
		var CommentList = _context.Comment
			.Include(x => x.OperatorCreate)
			.Include(x => x.OperatorModify)
			.ToList();

		return CommentList;
	}

	public async Task Update(Comment comment)
	{
		if (comment.Id != 0)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

		var Oldcomment = _context.Comment.First(x => x.Id == comment.Id);
		if (Oldcomment == null)
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
		Oldcomment.Content = comment.Content;
		Oldcomment.DateTimeModify = DateTime.Now;
		await _context.SaveChangesAsync();
	}
}
