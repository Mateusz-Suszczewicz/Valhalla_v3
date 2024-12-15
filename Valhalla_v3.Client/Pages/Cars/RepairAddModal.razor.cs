using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Client.Pages.Cars;

public partial class RepairAddModal
{
    private CarHistoryRepair formModel = new CarHistoryRepair();
    private List<Mechanic> ListMechanic = new();
    private bool isMechanicOpen = false;
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
        try
        {
            var response = await Http.GetFromJsonAsync<List<Mechanic>>(navigation.ToAbsoluteUri($"api/Mechanic"));
            if (response != null)
            {
                ListMechanic = response;
                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
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
        model.OperatorCreateId = 3;
        model.OperatorModifyId = 3;
        var json = JsonSerializer.Serialize(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await Http.PostAsync(navigation.ToAbsoluteUri($"api/Mechanic"), content);
            if (response.IsSuccessStatusCode)
            {
                LoadMechnic();
                CloseStation();

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        CloseStation();
    }
}
