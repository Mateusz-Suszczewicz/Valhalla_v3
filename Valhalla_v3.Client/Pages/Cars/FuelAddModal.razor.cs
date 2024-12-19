using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Valhalla_v3.Shared.CarHistory;
using static MudBlazor.CategoryTypes;
using Valhalla_v3.Shared.ToDo;

namespace Valhalla_v3.Client.Pages.Cars;

public partial class FuelAddModal
{
    private CarHistoryFuel formModel = new CarHistoryFuel();
    private List<GasStation> ListGasStation = new();
    private bool isGasStationOpen = false;
    private string ErrorMessage;
    private GasStation gasStation = new();

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
        await LoadGaStationAsync();
    }

    private async Task LoadGaStationAsync()
    {
        try
        {
            var response = await Http.GetAsync(navigation.ToAbsoluteUri($"api/GasStation"));
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    ListGasStation = await response.Content.ReadFromJsonAsync<List<GasStation>>() ?? new List<GasStation>();
                    ErrorMessage = string.Empty;
                    break;

                case System.Net.HttpStatusCode.NoContent:
                    return;

                default:
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage = $"Błąd API - {response.StatusCode}: {errorDetails}";
                    break;
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

    async Task OpenGasStation()
    {
        gasStation = new GasStation();
        isGasStationOpen = true;
        StateHasChanged();
    }

    async Task CloseGasStation()
    {
        isGasStationOpen = false;
        gasStation = new GasStation();
        StateHasChanged();
    }

    private async Task HandleGasStationSubmit(GasStation model)
    {
        try
        {
            var (success, error) = await apiService.PostAsync("api/gasStation", model);
            if (success)
            {
                await LoadGaStationAsync(); // Załaduj zaktualizowaną listę mechaników
                ErrorMessage = string.Empty;
            }
            else
            {
                ErrorMessage = error; // Obsłuż komunikat o błędzie
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Błąd aplikacji: {ex.Message} StackTrace: {ex.StackTrace}";
            Console.WriteLine(ex.Message);
        }
        finally
        {
            await CloseGasStation(); // Zamknij modal lub wykonaj inne działania końcowe
        }
    }
}
