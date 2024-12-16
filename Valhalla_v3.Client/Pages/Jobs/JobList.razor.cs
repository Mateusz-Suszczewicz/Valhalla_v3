using MudBlazor;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Valhalla_v3.Shared.ToDo;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Linq;

namespace Valhalla_v3.Client.Pages.Jobs;

public partial class JobList
{
    private List<Job> _items = new();
    private Job job = new();
    private bool IsCreateOpen = false;
    private bool IsTextOpen = false;
    private List<Project> projects = new();
    private int projectsId { get; set; }
    private int _id { get; set; }
    private bool? OnlyNoDoneJobs = true;
    private string ErrorMessage;
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
            if (response.IsSuccessStatusCode)
            {
                _items = await response.Content.ReadFromJsonAsync<List<Job>>();
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
        StateHasChanged();
    }

    private async Task LoadProject()
    {
        projects.Clear();
        try
        {
            var response = await Http.GetAsync(navigation.ToAbsoluteUri($"api/project"));
            if (response != null)
            {
                projects = await response.Content.ReadFromJsonAsync<List<Project>>();
                ErrorMessage = string.Empty;
                StateHasChanged();
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
                if (response.IsSuccessStatusCode)
                {
                    job = await response.Content.ReadFromJsonAsync<Job>();
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
        else
        {
            job = new Job();
            job.Term = DateTime.Now;
        }

        IsCreateOpen = true;
    }

    private void OpenTextModal()
    {
        IsTextOpen = true;
    }

    private async Task CreateProjekt(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            IsTextOpen = false;
            return;
        }
        Project project = new Project()
        {
            OperatorCreateId = 1,
            OperatorModifyId = 1,
            Name = name
        };
        var json = JsonSerializer.Serialize(project);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await Http.PostAsync(navigation.ToAbsoluteUri($"api/project"), content);
            if (response.IsSuccessStatusCode)
            {
                IsTextOpen = false;
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
        IsTextOpen = false;
    }

    private async Task SetProject()
    {
        if(projectsId != 0) 
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
        IsTextOpen = false;
    }

    private void ChangeDoneJobs()
    {
        OnlyNoDoneJobs = !OnlyNoDoneJobs;
        LoadJob();
    }
}
