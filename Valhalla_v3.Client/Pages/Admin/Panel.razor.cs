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
    private string ErrorMessage = string.Empty;

    private List<Project> projects = new List<Project>();
    private List<GasStation> gasStations = new List<GasStation>();
    private List<Mechanic> mechanics = new List<Mechanic>();
    private List<Operator> operators = new List<Operator>();

    private Project project = new Project();
    private bool isProjectOpen = false;
    private Mechanic mechanic = new Mechanic();
    private bool isMechanicOpen = false;
    private GasStation gasStation = new GasStation();
    private bool isGasStationOpen = false;
    private Operator oper = new Operator();
    private bool isOperatorOpen = false;

    protected override async Task OnInitializedAsync()
    {
        await LoadProjectsAsync();
        await LoadGasStationsAsync();
        await LoadMechanicsAsync();
        await LoadOperatorsAsync();
        StateHasChanged();
    }

    private async Task LoadDataAsync<T>(string endpoint, List<T> targetList)
    {
        targetList.Clear();
        try
        {
            var response = await Http.GetAsync(navigation.ToAbsoluteUri($"api/{endpoint}"));
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    targetList.AddRange(await response.Content.ReadFromJsonAsync<List<T>>() ?? new List<T>());
                    ErrorMessage = string.Empty;
                    break;

                case System.Net.HttpStatusCode.NoContent:
                    ErrorMessage = string.Empty;
                    break;

                default:
                    await HandleApiError(response);
                    break;
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }
    private async Task DeleteAsync<T>(int id)
    {
        if (id == 0)
        {
            ErrorMessage = "Nieprawidłowe ID";
            return;
        }

        var endpointInfo = GetEndpointInfo<T>();
        if (endpointInfo == null)
        {
            throw new InvalidOperationException("Unsupported type for processing");
        }

        string endpoint = endpointInfo.Value.Item1;
        try
        {
            var response = await Http.DeleteAsync(navigation.ToAbsoluteUri($"api/{endpoint}/{id}"));
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                case System.Net.HttpStatusCode.NoContent:
                    await endpointInfo.Value.Item2(); // Odświeżenie danych
                    ErrorMessage = string.Empty;
                    break;

                default:
                    await HandleApiError(response);
                    break;
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }
    private (string, Func<Task>)? GetEndpointInfo<T>()
    {
        var endpoints = new Dictionary<Type, (string, Func<Task>)>
        {
            { typeof(Project), ("project", LoadProjectsAsync) },
            { typeof(GasStation), ("gasStation", LoadGasStationsAsync) },
            { typeof(Mechanic), ("mechanic", LoadMechanicsAsync) },
            { typeof(Operator), ("operator", LoadOperatorsAsync) }
        };

        return endpoints.TryGetValue(typeof(T), out var endpointInfo) ? endpointInfo : null;
    }
    private async Task HandleApiError(HttpResponseMessage response)
    {
        var errorDetails = await response.Content.ReadAsStringAsync();
        ErrorMessage = $"Błąd API - {response.StatusCode}: {errorDetails}";
    }
    private void HandleException(Exception ex)
    {
        ErrorMessage = $"Błąd aplikacji: {ex.Message} StackTrace: {ex.StackTrace}";
        Console.WriteLine(ex.Message);
    }
    void CloseEntity<T>(Action<T> resetEntity, Action<bool> setIsOpen) where T : new()
    {
        resetEntity(new T());
        setIsOpen(false);
        StateHasChanged();
    }
    private async Task OpenEntity<T>(int id, Action<T> setEntity, Action<bool> setIsOpen, string endpoint) where T : new()
    {
        if (id != 0)
        {
            try
            {
                var (data, error) = await apiService.GetAsync<T>($"api/{endpoint}/{id}");

                if (string.IsNullOrEmpty(error))
                {
                    setEntity(data ?? new T());
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
        else
        {
            setEntity(new T());
        }

        setIsOpen(true);
    }
    private async Task HandleEntitySubmit<T>(T model, string endpoint, Func<Task> reloadEntities, Action<bool> setIsOpen) where T : new()
    {
        try
        {
            var (success, error) = await apiService.PostAsync($"api/{endpoint}", model);
            if (success)
            {
                await reloadEntities();
                ErrorMessage = string.Empty;
                
            }
            else
            {
                ErrorMessage = error;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Błąd aplikacji: {ex.Message} StackTrace: {ex.StackTrace}";
            Console.WriteLine(ex.Message);
        }
        setIsOpen(false);
    }

    private async Task LoadProjectsAsync() => await LoadDataAsync("project", projects);
    void CloseProject() => CloseEntity<Project>(newProject => project = newProject, isOpen => isProjectOpen = isOpen);
    private async Task OpenProject(int id) => await OpenEntity<Project>(id, newProject => project = newProject, isOpen => isProjectOpen = isOpen, "project");
    private async Task HandleProjectSubmit(Project model) => await HandleEntitySubmit(model, "project", LoadProjectsAsync, isOpen => isProjectOpen = isOpen);

    private async Task LoadMechanicsAsync() => await LoadDataAsync("mechanic", mechanics);
    void CloseMechanic() => CloseEntity<Mechanic>(newMechanic => mechanic = newMechanic, isOpen => isMechanicOpen = isOpen);
    private async Task OpenMechanic(int id) => await OpenEntity<Mechanic>(id, newMechanic => mechanic = newMechanic, isOpen => isMechanicOpen = isOpen, "mechanic");
    private async Task HandleMechanicSubmit(Mechanic model) => await HandleEntitySubmit(model, "mechanic", LoadMechanicsAsync, isOpen => isMechanicOpen = isOpen);

    private async Task LoadGasStationsAsync() => await LoadDataAsync("gasStation", gasStations);
    void CloseGasStation() => CloseEntity<GasStation>(newGasStation => gasStation = newGasStation, isOpen => isGasStationOpen = isOpen);
    private async Task OpenGasStation(int id) => await OpenEntity<GasStation>(id, newGasStation => gasStation = newGasStation, isOpen => isGasStationOpen = isOpen, "gasStation");
    private async Task HandleGasStationSubmit(GasStation model) => await HandleEntitySubmit(model, "gasStation", LoadGasStationsAsync, isOpen => isGasStationOpen = isOpen);


    private async Task LoadOperatorsAsync() => await LoadDataAsync("operator", operators);
    void CloseOperators() => CloseEntity<Operator>(newOperator => oper = newOperator, isOpen => isOperatorOpen = isOpen);
    private async Task OpenOperator(int id) => await OpenEntity<Operator>(id, newOperator => oper = newOperator, isOpen => isOperatorOpen = isOpen, "operator");
    private async Task HandleOperatorSubmit(Operator model) => await HandleEntitySubmit(model, "operator", LoadOperatorsAsync, isOpen => isOperatorOpen = isOpen);
}
