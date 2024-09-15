﻿using System.ComponentModel.DataAnnotations;

namespace Valhalla_v3.Shared.CarHistory;

public class CarHistoryRepair : MainClass
{
	public int CarId { get; set; }
	public int MechanicId { get; set; }
	public int Mileage { get; set; }
	public DateTime Date { get; set; }
	public decimal Cost { get; set; }
	[MaxLength(500)]
	public string? Description { get; set; }
	public virtual Mechanic? Mechanic { get; set; }
	public virtual Car Car { get; set; }

}
