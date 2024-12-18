using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Valhalla_v3.Shared.ToDo;

namespace Valhalla_v3.Shared.CarHistory;

public class CarHistoryFuel : MainClass
{
    [Required(ErrorMessage = "Pole Samochód jest wymagane.")]
    [Range(1, int.MaxValue, ErrorMessage = "Pole Samochód jest wymagane.")]
    public int CarId { get; set; }
    
    [Required(ErrorMessage = "Pole Przebieg jest wymagane.")]
    [Range(1, int.MaxValue, ErrorMessage = "Pole Przebieg jest wymagane.")]
    public int Mileage { get; set; }
    
    public DateTime Date { get; set; }
    
    [Required(ErrorMessage = "Pole Koszt jest wymagane.")]
    [Range(1, int.MaxValue, ErrorMessage = "Pole Koszt jest wymagane.")]
    public decimal Cost { get; set; }
    
    [Required(ErrorMessage = "Pole Stacja jest wymagane.")]
    [Range(1, int.MaxValue, ErrorMessage = "Pole Stacja jest wymagane.")]
    public int GasStationId { get; set; }
    
    [Required(ErrorMessage = "Pole Koszt za litr jest wymagane.")]
    [Range(1, int.MaxValue, ErrorMessage = "Pole Koszt za litr jest wymagane.")]
    public decimal CostPerLitr { get; set; }

    [JsonIgnore]
    public virtual GasStation? GasStation { get; set; }
	[JsonIgnore]
	public virtual Car? Car { get; set; }

}
