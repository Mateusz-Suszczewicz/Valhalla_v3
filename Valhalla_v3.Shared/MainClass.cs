using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Valhalla_v3.Shared;

public class MainClass
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
	public int Id { get; set; }

	public int OperatorCreateId { get; set; }
    public virtual Operator? OperatorCreate { get; set; }
	public DateTime DateTimeAdd { get; set; }
	
	public int OperatorModifyId { get; set; }
	public virtual Operator? OperatorModify { get; set; }
	public DateTime DateTimeModify { get; set; }
}
