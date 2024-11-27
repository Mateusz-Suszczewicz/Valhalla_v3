using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Shared.ToDo;

public  class Comment: MainClass
{
	[MaxLength(500)]
	public required string Content { get; set; }
	public int JobId { get; set; }

    [JsonIgnore]
    public virtual Job? Job { get; set; }
}
