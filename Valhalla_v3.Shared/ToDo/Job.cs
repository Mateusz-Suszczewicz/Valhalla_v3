using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Valhalla_v3.Shared.ToDo;

public class Job : MainClassFull
{
    public DateTime Term { get; set; }
	public bool IsCompleted { get; set; }

    [Required(ErrorMessage = "Pole Projekt jest wymagane.")]
    [Range(1, int.MaxValue, ErrorMessage = "Pole Projekt jest wymagane.")]
    public int ProjectId { get; set; }
    [JsonIgnore]
    public virtual Project? Project { get; set; }
	public virtual ICollection<Comment>? Comments { get; set; } = new List<Comment>();
}
