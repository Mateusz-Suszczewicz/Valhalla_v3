using System.ComponentModel.DataAnnotations;

namespace Valhalla_v3.Shared.ToDo;

public  class Comment: MainClass
{
	[MaxLength(500)]
	public required string Content { get; set; }
}
