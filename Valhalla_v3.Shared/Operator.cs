using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Valhalla_v3.Shared;

public class Operator
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }

    public DateTime DateTimeAdd { get; set; }
    public DateTime DateTimeModify { get; set; }

    [MaxLength(50)]
	public string Name { get; set; }
	[MaxLength(50)]
	public string? Password { get; set; }
}
