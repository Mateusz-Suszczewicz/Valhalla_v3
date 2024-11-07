using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Client.Pages.Cars;

public partial class GasStationAddModal
{
    private GasStation formModel = new GasStation();

    [Parameter]
    public EventCallback<GasStation> OnFormStationSubmit { get; set; }


    protected override async Task OnInitializedAsync()
    {

    }

    // Obsługa walidacji formularza i wywołanie callbacku
    private async Task HandleValidStationSubmit()
    {
        await OnFormStationSubmit.InvokeAsync(formModel);
    }
}