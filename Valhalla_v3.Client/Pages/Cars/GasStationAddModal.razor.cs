using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Client.Pages.Cars;

public partial class GasStationAddModal
{
    private GasStation formModel = new GasStation();
    private HubConnection _hubConnection;

    [Parameter]
    public EventCallback<GasStation> OnFormStationSubmit { get; set; }


    protected override async Task OnInitializedAsync()
    {
        _hubConnection = new HubConnectionBuilder()
        .WithUrl(navigation.ToAbsoluteUri("/carhub"))
        .Build();

        await _hubConnection.StartAsync();
        await _hubConnection.InvokeAsync("GetGasStation");
    }

    // Obsługa walidacji formularza i wywołanie callbacku
    private async Task HandleValidStationSubmit()
    {
        await OnFormStationSubmit.InvokeAsync(formModel);
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
    }

}