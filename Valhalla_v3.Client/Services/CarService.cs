using Valhalla_v3.Shared.CarHistory;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Components;

namespace Valhalla_v3.Client.Services;

public class CarService
{
    private HubConnection? hubConnection;
    private NavigationManager? navigationManager;

    public CarService(HubConnection? hubConnection, )
    {

    }

    public void getCarList()
    {
        hubConnection = new HubConnectionBuilder()
           .WithUrl(Navigation.ToAbsoluteUri("/myhub"))
           .Build();

        hubConnection.On<List<MyObject>>("ReceiveList", (objects) =>
        {
            receivedObjects = objects;
            StateHasChanged(); // Notify the component to re-render
        });

        await hubConnection.StartAsync();
    }
}
