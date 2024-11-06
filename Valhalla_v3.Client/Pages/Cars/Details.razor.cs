using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;
using Valhalla_v3.Shared;
using Valhalla_v3.Shared.CarHistory;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Valhalla_v3.Client.Pages.Cars;

public partial class Details
{
    [Parameter]
    public string Id { get; set; }

    private Tabs activeTab = Tabs.Details;
    private Car? car { get; set; }
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
        //TODO: Pobranie konkretnego auta
        await LoadCar();

    }
    private async Task LoadCar()
    {
        try
        {
            var response = await Http.GetFromJsonAsync<Car>(navigation.ToAbsoluteUri($"api/car/{Id}"));
            if (response != null)
            {
                car = response;
                ReloadDate();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    private void ReloadDate()
    {
        FuelCost = car.Fuels.Where(x => x.DateTimeModify.Month == DateTime.Now.Month && x.DateTimeModify.Year == DateTime.Now.Year).Sum(x => x.Cost);
        RepairCost = car.CarHistoryRepair.Where(x => x.Date.Month == DateTime.Now.Month && x.Date.Year == DateTime.Now.Year).Sum(x => x.Cost);
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
        //TODO: Dodanie tankowania

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
        //TODO: Dodatnie naprawy

        CloseRepair();
    }
}

public enum Tabs
{
    Details,
    Statystic
}
