namespace Valhalla_v3.Shared.ToDo;

public class Project : MainClassFull
{
    public virtual ICollection<Job> Tasks { get; set; } = new List<Job>();
}
