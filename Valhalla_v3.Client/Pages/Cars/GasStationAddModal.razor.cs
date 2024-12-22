using Microsoft.AspNetCore.Components;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Client.Pages.Cars;

public partial class GasStationAddModal
{
    [Parameter]
    public GasStation gasStation { get; set; } = new();

    [Parameter]
    public EventCallback<GasStation> StationSubmit { get; set; }


    protected override async Task OnInitializedAsync()
    {

    }

    // Obsługa walidacji formularza i wywołanie callbacku
    private async Task HandleValidStationSubmit()
    {
        await StationSubmit.InvokeAsync(gasStation);
        gasStation = new();
    }
}