using System.Text.Json.Serialization;
using Valhalla_v3.Shared.ToDo;

namespace Valhalla_v3.Shared.CarHistory;

public class CarHistoryFuel : MainClass
{
	public int CarId { get; set; }
	public int GasStationId { get; set; }
	public int Mileage { get; set; }
	public DateTime Date { get; set; }
	public decimal Cost { get; set; }
	public decimal CostPerLitr { get; set; }
	public virtual GasStation GasStation { get; set; }
	[JsonIgnore]
	public virtual Car Car { get; set; }

}
