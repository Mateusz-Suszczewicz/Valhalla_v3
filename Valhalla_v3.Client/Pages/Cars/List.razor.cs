using Microsoft.AspNetCore.SignalR.Client;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Client.Pages.Cars;

public partial class List
{
    private List<Car> messages = new();
    private HubConnection _hubConnection;

    protected override async Task OnInitializedAsync()
    {
        _hubConnection = new HubConnectionBuilder()
        .WithUrl(navigation.ToAbsoluteUri("/carhub"))
        .Build();

        _hubConnection.On<List<Car>>("CarList", (receivedItems) =>
        {
            messages = receivedItems;
            InvokeAsync(StateHasChanged);
        });

        await _hubConnection.StartAsync();
        await _hubConnection.InvokeAsync("SendMessage");

    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
    }

}
