using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Valhalla_v3.Shared.CarHistory;

public class GasStation : MainClassFull
{
    [MaxLength(38, ErrorMessage = "Pole Ulica może mieć maksymalnie 38 znaków.")]
    public string? Street { get; set; }
    [MaxLength(50, ErrorMessage = "Pole Numer może mieć maksymalnie 50 znaków.")]
    public string? StreetNumber { get; set; }
    [MaxLength(10, ErrorMessage = "Pole Kod pocztowy może mieć maksymalnie 10 znaków.")]
    public string? PostalCode { get; set; }
    [MaxLength(58, ErrorMessage = "Pole Miasto może mieć maksymalnie 58 znaków.")]
    public string? City { get; set; }
    [MaxLength(215, ErrorMessage = "Pole Państwo może mieć maksymalnie 215 znaków.")]
    public string? Country { get; set; }
    [MaxLength(20, ErrorMessage = "Pole Numer 1 może mieć maksymalnie 20 znaków.")]
    public string? Phone1 { get; set; }
    [MaxLength(20, ErrorMessage = "Pole Numer 2 może mieć maksymalnie 20 znaków.")]
    public string? Phone2 { get; set; }
    public bool Activ { get; set; }
    [JsonIgnore]
    public virtual ICollection<CarHistoryFuel> Fuels { get; set; } = new List<CarHistoryFuel>();
}
