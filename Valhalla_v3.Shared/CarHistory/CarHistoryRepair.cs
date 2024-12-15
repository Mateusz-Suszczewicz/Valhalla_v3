using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Valhalla_v3.Shared.CarHistory;

public class CarHistoryRepair : MainClass
{
    [Required(ErrorMessage = "Pole Samochód jest wymagane.")]
    public int CarId { get; set; }
    [Required(ErrorMessage = "Pole Mechanik jest wymagane.")]
    public int MechanicId { get; set; }
    [Required(ErrorMessage = "Pole Przebieg jest wymagane.")]
    public int Mileage { get; set; }
	public DateTime Date { get; set; }
    [Required(ErrorMessage = "Pole Koszt jest wymagane.")]
    public decimal Cost { get; set; }
    [StringLength(500, ErrorMessage = "Pole Opis może mięć maksymalnie 500 znaków.")]
    public string? Description { get; set; }
    [JsonIgnore]
    public virtual Mechanic? Mechanic { get; set; }
	[JsonIgnore]
	public virtual Car? Car { get; set; }

}
