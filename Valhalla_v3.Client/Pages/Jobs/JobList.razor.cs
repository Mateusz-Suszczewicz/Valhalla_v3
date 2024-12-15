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
    private List<Job> _items = new()
    {
        new(){ Name = "test1", Term = DateTime.Today, Id = 1 },
        new(){ Name = "test2", Term = DateTime.Today.AddDays(-1), Id= 2 }
    };
    private Job job = new();
    private bool IsCreateOpen = false;
    private bool IsTextOpen = false;
    private List<Project> projects = new();
    private int projectsId { get; set; }
    private int _id { get; set; }
    private bool OnlyNoDoneJobs = false;
    protected override async Task OnInitializedAsync()
    {
        LoadJob();
        LoadProject();
    }

    private async Task LoadJob()
    {
        _items.Clear();

        try
        {
            var response = await Http.GetFromJsonAsync<List<Job>>(navigation.ToAbsoluteUri($"api/job"));
            if (response != null)
            {
                _items.Clear();
                _items.AddRange(response);
                _items = _items.Where(x => x.Term <= DateTime.Now.Date).ToList();
                if(projectsId != 0)
                    _items = _items.Where(x => x.ProjectId == projectsId).ToList();
                if(OnlyNoDoneJobs)
                    _items = _items.Where(x => !x.IsCompleted).ToList();

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        StateHasChanged();
    }

    private async Task LoadProject()
    {
        _items.Clear();
        try
        {
            var response = await Http.GetFromJsonAsync<List<Project>>(navigation.ToAbsoluteUri($"api/project"));
            if (response != null)
            {
                projects = response;
                StateHasChanged();
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
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
                IsCreateOpen = false;
                await LoadJob();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void CloseModal(bool close)
    {
        IsCreateOpen = close;
        job = new Job();
    }

    private async Task OpemModal(int id)
    {
        if (id != 0) 
        { 
            try
            {
                var response = await Http.GetFromJsonAsync<Job>(navigation.ToAbsoluteUri($"api/job/{id}"));
                if (response != null)
                {
                    job = response;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            job = new Job();
            job.Term = DateTime.Now.Date;
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
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        IsTextOpen = false;
    }

    private async Task SetProject()
    {
        if(projectsId != 0) 
            await LoadJob();
    }

    private void DragStartHandler(DragEventArgs e, Job item)
    {
        item.Term = DateTime.Now.Date;
        Console.WriteLine($"Rozpoczęto przeciąganie: {item}");
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

}
