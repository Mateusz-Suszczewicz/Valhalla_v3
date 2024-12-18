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
    private string ErrorMessage = string.Empty;
    
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
        try
        {
            var response = await Http.GetAsync(navigation.ToAbsoluteUri($"api/gasStations"));
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
        try
        {
            var response = await Http.GetAsync(navigation.ToAbsoluteUri($"api/mechanics"));
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
        try
        {
            var response = await Http.GetAsync(navigation.ToAbsoluteUri($"api/operators"));
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
}
