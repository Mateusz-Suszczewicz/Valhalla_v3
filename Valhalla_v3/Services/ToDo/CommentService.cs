using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Web.Http;
using Valhalla_v3.Database;
using Valhalla_v3.Shared.CarHistory;
using Valhalla_v3.Shared.ToDo;

namespace Valhalla_v3.Services.ToDo;

public interface ICommentService
{
	Task<int> Create(Comment comment);
	Task<Comment> Get(int id);
	Task<List<Comment>> Get();
	Task Update(Comment comment);
	Task Delete(int id);
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
        if (comment == null)
            throw new ArgumentNullException(nameof(comment), "Comment object cannot be null.");

        if (comment.Id != 0)
            throw new ArgumentException("Comment ID must be 0 for a new entry.");

        comment.DateTimeAdd = DateTime.Now;
        comment.DateTimeModify = DateTime.Now;

        await _context.Comment.AddAsync(comment);
        await _context.SaveChangesAsync();

        return comment.Id;
    }

    public async Task Delete(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var comment = await _context.Comment.FirstOrDefaultAsync(x => x.Id == id);

        if (comment == null)
            throw new KeyNotFoundException($"Comment with ID {id} not found.");

        _context.Comment.Remove(comment);
        await _context.SaveChangesAsync();
    }

    public async Task<Comment> Get(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var comment = await _context.Comment
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (comment == null)
            throw new KeyNotFoundException($"Comment with ID {id} not found.");

        return comment;
    }

    public async Task<List<Comment>> Get()
    {
        var commentList = await _context.Comment
            .Include(x => x.OperatorCreate)
            .Include(x => x.OperatorModify)
            .ToListAsync();

        return commentList ?? new List<Comment>();
    }

    public async Task Update(Comment comment)
    {
        if (comment == null)
            throw new ArgumentNullException(nameof(comment), "Comment object cannot be null.");

        if (comment.Id <= 0)
            throw new ArgumentException("Invalid ID. ID must be greater than zero.");

        var existingComment = await _context.Comment.FirstOrDefaultAsync(x => x.Id == comment.Id);

        if (existingComment == null)
            throw new KeyNotFoundException($"Comment with ID {comment.Id} not found.");

        existingComment.Content = comment.Content;
        existingComment.DateTimeModify = DateTime.Now;

        await _context.SaveChangesAsync();
    }
}

