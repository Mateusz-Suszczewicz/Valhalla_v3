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
    private bool IsCreateOpen = false;
    private bool IsTextOpen = false;
    private List<Project> projects = new();
    private int projectsId { get; set; }
    private int _id { get; set; }
    private Job draggedItem = null;

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
                _items.AddRange(response);
                //_items = _items.Where(x => x.ProjectId == projectsId && x.Term <= DateTime.Now.Date).ToList();
                if (projectsId != 0)
                    _items.Add(new Job() { Name = "nowy", Term = DateTime.Now.Date });
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
        job.OperatorCreateId = 3;
        job.OperatorModifyId = 3;
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

    private void CloseModal()
    {
        IsCreateOpen = false;
    }
    
    private void OpemModal(int id)
    {
        _id = id;
        IsCreateOpen = true;
    }

    private void OpenTextModal()
    {
        IsTextOpen = true;
    }

    private async Task CreateProjekt(string name)
    {
        Project project = new Project()
        {
            OperatorCreateId = 3,
            OperatorModifyId = 3,
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
    }

    private async Task SetProject()
    {
        if(projectsId != 0) 
            await LoadJob();
    }

    private void DragStartHandler(DragEventArgs e, Job item)
    {
        // Ustawiamy przeciągany element w zmiennej
        item.Term = DateTime.Now.Date.AddDays(-1);
        Console.WriteLine($"Rozpoczęto przeciąganie: {item}");
    }

    //private void DropHandler(DragEventArgs e, int targetList)
    //{
    //    // Sprawdzamy, gdzie upuszczono element
    //    if (draggedItem != null)
    //    {
            
    //    }

    //    // Resetujemy zmienną przeciąganego elementu
    //    draggedItem = null;

    //    Console.WriteLine($"Element upuszczony do listy {targetList}");
    //}
}
