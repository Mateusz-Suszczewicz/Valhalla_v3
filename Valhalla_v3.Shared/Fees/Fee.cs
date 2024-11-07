namespace Valhalla_v3.Shared.Fees;

public class Fee : MainClass
{
    public string Name { get; set; }
    public DateTime DateFee { get; set; }
    public decimal DefultAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public bool Paid { get; set; }
}