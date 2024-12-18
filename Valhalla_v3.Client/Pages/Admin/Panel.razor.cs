using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json;
using System.Text;
using Valhalla_v3.Shared;
using Valhalla_v3.Shared.CarHistory;
using Valhalla_v3.Shared.ToDo;
using static System.Net.WebRequestMethods;
using static MudBlazor.CategoryTypes;
using System.Net.Http.Json;

namespace Valhalla_v3.Client.Pages.Admin;

public partial class Panel
{
    private List<Project> projects = new List<Project>();
    private List<GasStation> gasStations = new List<GasStation>();
    private List<Mechanic> mechanics = new List<Mechanic>();
    private List<Operator> operators = new List<Operator>();

    private Mechanic mechanic = new Mechanic();

    private string ErrorMessage = string.Empty;
    private bool isMechanicOpen = false;

    protected override async Task OnInitializedAsync()
    {
        await LoadProjectsAsync();
        await LoadGasStationssAsync();
        await LoadMechanicsAsync();
        await LoadOperatorsAsync();
        StateHasChanged();
    }
    
    private async Task LoadProjectsAsync()
    {
        projects.Clear();
        try
        {
            var response = await Http.GetAsync(navigation.ToAbsoluteUri($"api/project"));
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    projects = await response.Content.ReadFromJsonAsync<List<Project>>() ?? new List<Project>();
                    ErrorMessage = string.Empty;
                    break;

                case System.Net.HttpStatusCode.NoContent:
                    projects = new List<Project>();
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

    private async Task LoadGasStationssAsync()
    {
        gasStations.Clear();
        try
        {
            var response = await Http.GetAsync(navigation.ToAbsoluteUri($"api/gasStation"));
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    gasStations = await response.Content.ReadFromJsonAsync<List<GasStation>>() ?? new List<GasStation>();
                    ErrorMessage = string.Empty;
                    break;

                case System.Net.HttpStatusCode.NoContent:
                    projects = new List<Project>();
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

    private async Task LoadMechanicsAsync()
    {
        mechanics.Clear();
        try
        {
            var response = await Http.GetAsync(navigation.ToAbsoluteUri($"api/mechanic"));
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    mechanics = await response.Content.ReadFromJsonAsync<List<Mechanic>>() ?? new List<Mechanic>();
                    ErrorMessage = string.Empty;
                    break;

                case System.Net.HttpStatusCode.NoContent:
                    projects = new List<Project>();
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

    private async Task LoadOperatorsAsync()
    {
        operators.Clear();
        try
        {
            var response = await Http.GetAsync(navigation.ToAbsoluteUri($"api/operator"));
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    operators = await response.Content.ReadFromJsonAsync<List<Operator>>() ?? new List<Operator>();
                    ErrorMessage = string.Empty;
                    break;

                case System.Net.HttpStatusCode.NoContent:
                    projects = new List<Project>();
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

    private async Task DeleteProjectAsync(int id)
    {
        if (id != 0) return;

        try
        {
            var response = await Http.DeleteAsync(navigation.ToAbsoluteUri($"api/project/{id}"));
            if (response.IsSuccessStatusCode)
            {
                await LoadProjectsAsync();
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
    
    private async Task DeleteGasStationAsync(int id)
    {
        if (id != 0) return;

        try
        {
            var response = await Http.DeleteAsync(navigation.ToAbsoluteUri($"api/gastation/{id}"));
            if (response.IsSuccessStatusCode)
            {
                await LoadGasStationssAsync();
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

    private async Task DeleteMechanicAsync(int id)
    {
        if (id != 0) return;

        try
        {
            var response = await Http.DeleteAsync(navigation.ToAbsoluteUri($"api/mechanic/{id}"));
            if (response.IsSuccessStatusCode)
            {
                await LoadMechanicsAsync();
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

    private async Task DeleteOperatorAsync(int id)
    {
        if (id != 0) return;

        try
        {
            var response = await Http.DeleteAsync(navigation.ToAbsoluteUri($"api/operator/{id}"));
            if (response.IsSuccessStatusCode)
            {
                await LoadOperatorsAsync();
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

    void CloseMechanic()
    {
        isMechanicOpen = false;
        StateHasChanged();
    }

    async Task OpenMechanic(int id) 
    {
        if (id != 0)
        {
            try
            {
                var response = await Http.GetAsync(navigation.ToAbsoluteUri($"api/mechanic/{id}"));
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        mechanic = await response.Content.ReadFromJsonAsync<Mechanic>() ?? new Mechanic();
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
        else
        {
            mechanic = new Mechanic();
        }

        isMechanicOpen = true;
    }

    private async Task HandleMechanicSubmit(Mechanic model)
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
                await LoadMechanicsAsync();
                CloseMechanic();
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
        CloseMechanic();
    }
}
