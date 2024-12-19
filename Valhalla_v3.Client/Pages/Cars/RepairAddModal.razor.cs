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
    private Mechanic mechanic = new();

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
        await LoadMechanicsAsync();
    }

    private async Task LoadMechanicsAsync()
    {
        try
        {
            var (mechanics, error) = await apiService.GetListAsync<Mechanic>("api/Mechanic");

            if (string.IsNullOrEmpty(error))
            {
                ListMechanic = mechanics ?? new List<Mechanic>();
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
    }

    private async Task HandleValidMechanicSubmit()
    {
        await OnFormSubmit.InvokeAsync(formModel);
        formModel = new();
    }

    async Task OpenMechanic()
    {
        mechanic = new Mechanic();
        isMechanicOpen = true;
        StateHasChanged();
    }

    async Task CloseMechanic()
    {
        isMechanicOpen = false;
        mechanic = new Mechanic();
        StateHasChanged();
    }

    private async Task HandleMechanicSubmit(Mechanic model)
    {
        try
        {
            var (success, error) = await apiService.PostAsync("api/Mechanic", model);
            if (success)
            {
                await LoadMechanicsAsync(); // Załaduj zaktualizowaną listę mechaników
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
            CloseMechanic(); // Zamknij modal lub wykonaj inne działania końcowe
        }
    }
}
