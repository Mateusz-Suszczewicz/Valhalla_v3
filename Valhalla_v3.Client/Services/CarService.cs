using Valhalla_v3.Shared.CarHistory;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Valhalla_v3.Client.Services;
public interface ICarClient : IAsyncDisposable
{
    public event Action<List<Car>> OnReceiveMessage;

    public ValueTask DisposeAsync();
    public Task InitializeAsync();

}
public class CarClient : ICarClient
{
    private readonly NavigationManager _navigationManager;
    private HubConnection _hubConnection;

    public event Action<List<Car>> OnReceiveMessage;

    public CarClient(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    public async Task InitializeAsync()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_navigationManager.ToAbsoluteUri("/carhub"))
            .Build();

        await _hubConnection.StartAsync();

        _hubConnection.On<List<Car>>("CarList", (car) =>
        {
            OnReceiveMessage?.Invoke(car);
        });
        await _hubConnection.SendAsync("SendMessage");

    }

    public async Task SendMessageAsync(string user, string message)
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.SendAsync("SendMessage", user, message);
        }
    }

    public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }
    }
}
