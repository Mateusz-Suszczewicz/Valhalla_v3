namespace Valhalla_v3.Client.Pages.Fees;

public partial class HistoricalOverview
{
    public List<int> year { get; set; }

    protected override async Task OnInitializedAsync()
    {
        year = new List<int>() { 2022, 2023, 2024 };
        //TODO: dodać do serwisu wyciąganie lat z jakich mamy danes
    }
}
