using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Client.Pages.Cars;

public partial class FuelAddModal
{
    private CarHistoryFuel formModel = new CarHistoryFuel();
    private List<GasStation> ListGasStation = new();
    private HubConnection _hubConnection;
    private bool isGasSttionOpen = false;
    [Parameter]
    public EventCallback<CarHistoryFuel> OnFormFuelSubmit { get; set; }


    protected override async Task OnInitializedAsync()
    {
        _hubConnection = new HubConnectionBuilder()
        .WithUrl(navigation.ToAbsoluteUri("/carhub"))
        .Build();

        _hubConnection.On<List<GasStation>>("GasStation", (receivedItem) =>
        {
            ListGasStation = receivedItem;
            InvokeAsync(StateHasChanged);
        });

        await _hubConnection.StartAsync();
        await _hubConnection.InvokeAsync("GetGasStation");
    }

    // Obsługa walidacji formularza i wywołanie callbacku
    private async Task HandleValidFuelSubmit()
    {
        await OnFormFuelSubmit.InvokeAsync(formModel);
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
    }

    void OpenStation()
    {
        isGasSttionOpen = true;
        StateHasChanged();
    }

    async Task CloseStation()
    {
        await _hubConnection.InvokeAsync("GetGasStation");
        isGasSttionOpen = false;
        StateHasChanged();
    }

    private async void HandleStationSubmit(GasStation model)
    {
        await _hubConnection.SendAsync("AddGasStation", model);
        CloseStation();
    }
}
