using Microsoft.AspNetCore.Components;

namespace Valhalla_v3.Client.Pages.Fees;

public partial class FeeList
{
    [Parameter]
    public int Id { get; set; }
    private Tabs activeTab = Tabs.currentMonth;
    

    protected override async Task OnInitializedAsync()
    {

    }

    private void SelectTab(Tabs tab)
    {
        activeTab = tab;
        StateHasChanged();
    }
}
