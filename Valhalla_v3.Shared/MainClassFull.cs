using System.ComponentModel.DataAnnotations;

namespace Valhalla_v3.Shared;

public class MainClassFull : MainClass
{
	[StringLength(100)]
	public string Name { get; set; }

	[StringLength(500)]
	public string? Description { get; set; }
}
