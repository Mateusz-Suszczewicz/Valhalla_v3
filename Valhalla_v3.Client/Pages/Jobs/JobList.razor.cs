using MudBlazor;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Valhalla_v3.Shared.ToDo;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Linq;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Client.Pages.Jobs;

public partial class JobList
{
    private List<Job> _items = new();
    private Job job = new();
    private bool IsCreateOpen = false;
    private bool IsPProjectOpen = false;
    private List<Project> projects = new();
    private int projectsId { get; set; } = new();
    private int _id { get; set; }
    private bool? OnlyNoDoneJobs = true;
    private string ErrorMessage;
    private Project project { get; set; } = new Project();
    protected override async Task OnInitializedAsync()
    {
        LoadJob();
        LoadProject();
    }

    private async Task LoadJob()
    {
        _items.Clear();

        var queryParam = OnlyNoDoneJobs.HasValue
            ? $"?NoDoneJobs={OnlyNoDoneJobs.Value.ToString().ToLower()}&ProjectId={projectsId}"
            : string.Empty;
        
        try
        {
            var response = await Http.GetAsync(navigation.ToAbsoluteUri($"api/job{queryParam}"));
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    _items = await response.Content.ReadFromJsonAsync<List<Job>>() ?? new List<Job>();
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
        StateHasChanged();
    }

    private async Task LoadProject()
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
        StateHasChanged();
    }

    private async Task Create(Job job)
    {
        job.OperatorCreateId = 1;
        job.OperatorModifyId = 1;
        var json = JsonSerializer.Serialize(job);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await Http.PostAsync(navigation.ToAbsoluteUri($"api/job"), content);
            if (response.IsSuccessStatusCode)
            {
                CloseModal(false);
                await LoadJob();
                ErrorMessage = string.Empty ;
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

    private void CloseModal(bool close)
    {
        IsCreateOpen = close;
        job = new Job();
        StateHasChanged();
    }

    private async Task OpemModal(int id)
    {
        if (id != 0) 
        { 
            try
            {
                var response = await Http.GetAsync(navigation.ToAbsoluteUri($"api/job/{id}"));
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        job = await response.Content.ReadFromJsonAsync<Job>() ?? new Job();
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
            job = new Job();
            job.Term = DateTime.Now;
            job.ProjectId = projectsId;
        }

        IsCreateOpen = true;
    }

    private void OpenProject()
    {
        project = new Project();
        IsPProjectOpen = true;
    }

    private async Task CreateProjekt(Project project)
    {

        project.OperatorCreateId = 1;
        project.OperatorModifyId = 1;

        var json = JsonSerializer.Serialize(project);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await Http.PostAsync(navigation.ToAbsoluteUri($"api/project"), content);
            if (response.IsSuccessStatusCode)
            {
                IsPProjectOpen = false;
                await LoadProject();
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
        IsPProjectOpen = false;
    }

    private async Task SetProject()
    {
        await LoadJob();
    }

    private async Task DragStartHandler(DragEventArgs e, Job item)
    {
        item.Term = DateTime.Now.Date;
        var queryParam = $"?Id={item.Id}&newTerm={DateTime.Now.Date.ToString("yyyy-MM-ddTHH:mm:ss")}";

        try
        {
            var response = await Http.PostAsync(navigation.ToAbsoluteUri($"api/job/changeTerm{queryParam}"), null);
            if (response.IsSuccessStatusCode)
            {
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

    private void SetProjectDone(int id)
    {
        var item = _items.Where(x => x.Id == id).First();
        item.IsCompleted = !item.IsCompleted;
        Create(item);
    }
    
    private void CloseProject()
    {
        IsPProjectOpen = false;
    }

    private void ChangeDoneJobs()
    {
        OnlyNoDoneJobs = !OnlyNoDoneJobs;
        LoadJob();
    }
}
