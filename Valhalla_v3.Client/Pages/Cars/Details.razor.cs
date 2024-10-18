using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Valhalla_v3.Shared;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Client.Pages.Cars;

public partial class Details
{
    [Parameter]
    public string Id { get; set; }

    private Tabs activeTab = Tabs.Details;
    private Car? car { get; set; }
    private HubConnection _hubConnection;
    private int mileage { get; set; }
    private decimal FuelCost { get; set; }
    private decimal RepairCost { get; set; }
    private bool isFuelOpen = false;
    private bool isRapairOpen = false;

    private void SelectTab(Tabs tab)
    {
        activeTab = tab;
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        Operator oper = new() { Name = "admin", Id = 3 };
        car = new() { OperatorCreate = oper, OperatorModify = oper };

        _hubConnection = new HubConnectionBuilder()
        .WithUrl(navigation.ToAbsoluteUri("/carhub"))
        .Build();

        _hubConnection.On<Car>("Car", (receivedItem) =>
        {
                car = receivedItem;
                ReloadDate();
                InvokeAsync(StateHasChanged);
        });

        await _hubConnection.StartAsync();
        await _hubConnection.InvokeAsync("GetCar", Id);

    }

    private void ReloadDate()
    {
        FuelCost = car.Fuels.Where(x => x.DateTimeModify.Month == DateTime.Now.Month && x.DateTimeModify.Year == DateTime.Now.Year).Sum(x => x.Cost);
        RepairCost = car.CarHistoryRepair.Where(x => x.Date.Month == DateTime.Now.Month && x.Date.Year == DateTime.Now.Year).Sum(x => x.Cost);
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
    }

    void OpenFuel()
    {
        isFuelOpen = true;
        StateHasChanged();
    }

    void CloseFuel()
    {
        isFuelOpen = false;
        StateHasChanged();
    }

    private async void HandleFuelSubmit(CarHistoryFuel model)
    {
        model.CarId = car.Id;
        model.OperatorCreateId = 3;
        model.OperatorModifyId = 3;
        await _hubConnection.SendAsync("AddFuel", model);
        await _hubConnection.InvokeAsync("GetCar", Id);

        CloseFuel();
    }

    void OpenRepair()
    {
        isRapairOpen = true;
        StateHasChanged();
    }

    void CloseRepair()
    {
        isRapairOpen = false;
        StateHasChanged();
    }

    private async void HandleRepairSubmit(CarHistoryRepair model)
    {
        model.CarId = car.Id;
        model.OperatorCreateId = 3;
        model.OperatorModifyId = 3;
        await _hubConnection.SendAsync("AddRepair", model);
        await _hubConnection.InvokeAsync("GetCar", Id);

        CloseRepair();
    }
}

public enum Tabs
{
    Details,
    Statystic
}
