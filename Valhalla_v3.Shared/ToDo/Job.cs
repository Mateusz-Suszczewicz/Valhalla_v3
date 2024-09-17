namespace Valhalla_v3.Shared.ToDo;

public class Job : MainClassFull
{
    public DateTime? Term { get; set; }
	public bool IsCompleted { get; set; }

	public int ProjectId { get; set; }
	public virtual Project Project { get; set; }
	public virtual ICollection<Comment>? Comments { get; set; }
}
