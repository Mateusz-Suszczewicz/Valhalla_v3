using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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
    private string ErrorMessage;
    [Parameter]
    public int CarId
    {
        set
        {
            formModel.CarId = value;
        }
    }
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
            var response = await Http.GetAsync(navigation.ToAbsoluteUri($"api/GasStation"));
            if (response.IsSuccessStatusCode)
            {
                ListGasStation = await response.Content.ReadFromJsonAsync<List<GasStation>>();
                ErrorMessage = string.Empty;
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                ErrorMessage = $"Błąd API - {response.StatusCode}: {errorDetails}";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Błąd aplikacji: {ex.Message} StackTrace: {ex.StackTrace}";
            Console.WriteLine(ex.Message);
        }
    }
    
    private async Task HandleValidFuelSubmit()
    {
        Console.WriteLine("wywołanie metody");
        await OnFormFuelSubmit.InvokeAsync(formModel);
        formModel = new();
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
        model.OperatorCreateId = 3;
        model.OperatorModifyId = 3;
        var json = JsonSerializer.Serialize(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await Http.PostAsync(navigation.ToAbsoluteUri($"api/GasStation"), content);
            if (response.IsSuccessStatusCode)
            {
                LoadGaStation();
                CloseStation();
                ErrorMessage = string.Empty;
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                ErrorMessage = $"Błąd API - {response.StatusCode}: {errorDetails}";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Błąd aplikacji: {ex.Message} StackTrace: {ex.StackTrace}";
            Console.WriteLine(ex.Message);
        }
    }
}
