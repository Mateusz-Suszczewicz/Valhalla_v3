using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Client.Pages.Cars;

public partial class FuelAddModal
{
    private CarHistoryFuel formModel = new CarHistoryFuel();
    private List<GasStation> ListGasStation = new();
    private bool isGasSttionOpen = false;
    [Parameter]
    public EventCallback<CarHistoryFuel> OnFormFuelSubmit { get; set; }


    protected override async Task OnInitializedAsync()
    {
        //TODO: Pobranie stacji benzynowych
    }

    // Obsługa walidacji formularza i wywołanie callbacku
    private async Task HandleValidFuelSubmit()
    {
        await OnFormFuelSubmit.InvokeAsync(formModel);
    }

    void OpenStation()
    {
        isGasSttionOpen = true;
        StateHasChanged();
    }

    async Task CloseStation()
    {
        isGasSttionOpen = false;
        StateHasChanged();
    }

    private async void HandleStationSubmit(GasStation model)
    {
        //TODO: Dodanie stacji benzynowej
        CloseStation();
    }
}
