using System.ComponentModel.DataAnnotations;

namespace Valhalla_v3.Shared;

public class Operator : MainClass
{
	[MaxLength(50)]
	public required string Name { get; set; }
	[MaxLength(50)]
	public string? Password { get; set; }
}
