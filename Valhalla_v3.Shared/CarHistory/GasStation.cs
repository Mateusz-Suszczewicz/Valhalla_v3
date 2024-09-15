using System.ComponentModel.DataAnnotations;

namespace Valhalla_v3.Shared.CarHistory;

public class GasStation : MainClassFull
{
	[MaxLength(50)]
	public string Street { get; set; }
	[MaxLength(50)]
	public string StreetNumber { get; set; }
	[MaxLength(50)]
	public string PostalCode { get; set; }
	[MaxLength(50)]
	public string City { get; set; }
	[MaxLength(50)]
	public string Country { get; set; }
	[MaxLength(50)]
	public string Phone1 { get; set; }
	[MaxLength(50)]
	public string Phone2 { get; set; }

	public virtual ICollection<CarHistoryFuel> Fuels { get; set; } = new List<CarHistoryFuel>();
}
