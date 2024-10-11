using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Valhalla_v3.Client.Services;
using Valhalla_v3.Shared;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Client.Pages.Cars;

public partial class CarList
{
    private List<Car> messages = new();
    private string test = "Nic nie załadowano";
    private HubConnection _hubConnection;
    private bool load = true;
    //protected override async Task OnInitializedAsync()
    //{
    //    //carClient.OnReceiveMessage += HandleReceiveMessage;
    //    //await carClient.InitializeAsync();
    //    _hubConnection = new HubConnectionBuilder()
    //        .WithUrl(Navigation.ToAbsoluteUri("/carhub"))
    //        .Build();
    //    Console.WriteLine($"id: qwe");
    //    Operator oper = new() { Name = "test" };
    //    messages.Add(new Car() { Id = -1, OperatorCreate = oper, OperatorModify = oper });
    //    // Obsługa odbierania elementów z serwera
    //    _hubConnection.On<List<Car>>("CarList", (receivedItems) =>
    //    {
    //        messages = receivedItems;
    //        Console.WriteLine($"id: {messages[0].Id}");
    //        InvokeAsync(StateHasChanged);
    //    });

    //    // Uruchomienie połączenia
    //    await _hubConnection.StartAsync();
    //    await _hubConnection.SendAsync("SendMessage");
    //    StateHasChanged();

    //}

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
    }

    public async Task Otwarcie()
    {


    }
}
