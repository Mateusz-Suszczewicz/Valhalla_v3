namespace Valhalla_v3.Shared.ToDo;

public class Project : MainClassFull
{
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
