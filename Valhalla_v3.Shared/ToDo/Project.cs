using System.Text.Json.Serialization;

namespace Valhalla_v3.Shared.ToDo;

public class Project : MainClassFull
{
    [JsonIgnore]
    public virtual ICollection<Job> Tasks { get; set; } = new List<Job>();
}
