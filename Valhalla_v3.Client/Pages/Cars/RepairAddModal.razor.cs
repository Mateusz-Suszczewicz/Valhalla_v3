using Azure;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Valhalla_v3.Shared.CarHistory;
using static MudBlazor.CategoryTypes;
using Valhalla_v3.Shared.ToDo;

namespace Valhalla_v3.Client.Pages.Cars;

public partial class RepairAddModal
{
    private CarHistoryRepair formModel = new CarHistoryRepair();
    private List<Mechanic> ListMechanic = new();
    private bool isMechanicOpen = false;
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
    public EventCallback<CarHistoryRepair> OnFormSubmit { get; set; }


    protected override async Task OnInitializedAsync()
    {
        await LoadMechnic();
    }

    private async Task LoadMechnic()
    {
        ListMechanic.Clear();
        try
        {
            var response = await Http.GetAsync(navigation.ToAbsoluteUri($"api/Mechanic"));
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    ListMechanic = await response.Content.ReadFromJsonAsync<List<Mechanic>>() ?? new List<Mechanic>();
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

    // Obsługa walidacji formularza i wywołanie callbacku
    private async Task HandleValidMechanicSubmit()
    {
        await OnFormSubmit.InvokeAsync(formModel);
        formModel = new();
    }


    void OpenStation()
    {
        isMechanicOpen = true;
        StateHasChanged();
    }

    async Task CloseStation()
    {

        isMechanicOpen = false;
        StateHasChanged();
    }

    private async void HandleMechanicSubmit(Mechanic model)
    {
        model.OperatorCreateId = 1;
        model.OperatorModifyId = 1;
        var json = JsonSerializer.Serialize(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await Http.PostAsync(navigation.ToAbsoluteUri($"api/Mechanic"), content);
            if (response.IsSuccessStatusCode)
            {
                LoadMechnic();
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
        CloseStation();
    }
}
