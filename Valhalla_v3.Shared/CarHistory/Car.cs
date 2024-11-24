using System.ComponentModel.DataAnnotations;

namespace Valhalla_v3.Shared.CarHistory;

public class Car : MainClass
{
	[MaxLength(100)]
	public string Brand { get; set; }
	[MaxLength(100)]
	public string Model { get; set; }
	public int EngineCC { get; set; }
	public int Year { get; set; }
	[MaxLength(100)]
	public string VIN { get; set; }
	public DateTime? InsuranceDate { get; set; }
    public decimal? InsuranceCost { get; set; }
	public DateTime? SurveyDate { get; set; }
    public decimal? SurveyCost { get; set; }




    public virtual ICollection<CarHistoryRepair> CarHistoryRepair { get; set; } = new List<CarHistoryRepair>();
	public virtual ICollection<CarHistoryFuel> Fuels { get; set; } = new List<CarHistoryFuel>();
}
