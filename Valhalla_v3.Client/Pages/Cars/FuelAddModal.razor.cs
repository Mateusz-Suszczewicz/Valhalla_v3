using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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
        await LoadGaStation();
    }

    private async Task LoadGaStation()
    {
        try
        {
            var response = await Http.GetFromJsonAsync<List<GasStation>>(navigation.ToAbsoluteUri($"api/GasStation"));
            if (response != null)
            {
                ListGasStation = response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    private async Task HandleValidFuelSubmit()
    {
        await OnFormFuelSubmit.InvokeAsync(formModel);
    }

    private void OpenStation()
    {
        isGasSttionOpen = true;
        StateHasChanged();
    }

    private async Task CloseStation()
    {
        isGasSttionOpen = false;
        StateHasChanged();
    }

    private async Task HandleStationSubmit(GasStation model)
    {
        model.OperatorCreateId = 1;
        model.OperatorModifyId = 1;
        var json = JsonSerializer.Serialize(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await Http.PostAsync(navigation.ToAbsoluteUri($"api/GasStation"), content);
            if (response.IsSuccessStatusCode)
            {
                LoadGaStation();
                CloseStation();

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
