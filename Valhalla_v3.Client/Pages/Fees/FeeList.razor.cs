namespace Valhalla_v3.Client.Pages.Fees;

public partial class FeeList
{
    private Tabs activeTab = Tabs.currentMonth;
    
    private void SelectTab(Tabs tab)
    {
        activeTab = tab;
        StateHasChanged();
    }
}
