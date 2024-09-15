using System.ComponentModel.DataAnnotations;

namespace Valhalla_v3.Shared;

public class MainClass
{
	[Key]
	public int Id { get; set; }

	public int OperatorCreateId { get; set; }
	public virtual required Operator OperatorCreate { get; set; }
	public DateTime DateTimeAdd { get; set; }
	
	public int OperatorModifyId { get; set; }
	public virtual required Operator OperatorModify { get; set; }
	public DateTime DateTimeModify { get; set; }
}
