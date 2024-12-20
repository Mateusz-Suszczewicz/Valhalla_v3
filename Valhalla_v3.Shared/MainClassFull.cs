using System.ComponentModel.DataAnnotations;

namespace Valhalla_v3.Shared;

public class MainClassFull : MainClass
{
    [Required(ErrorMessage = "Pole Nazwa jest wymagane.")]
    [StringLength(100, ErrorMessage ="Pole Nazwa może mięć maksymalnie 100 znaków.")]
	public string Name { get; set; }

	[StringLength(1500, ErrorMessage = "Pole Opis może mięć maksymalnie 1500 znaków.")]
	public string? Description { get; set; }
}
