namespace Valhalla_v3.Shared.Fees;

public class FeeConfig : MainClass
{
    public string Name { get; set; }
    public decimal DefultAmount { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public bool disable { get; set; }
    public int CycleOf { get; set; }
}