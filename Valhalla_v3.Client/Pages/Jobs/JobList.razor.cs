using MudBlazor;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using Valhalla_v3.Shared.ToDo;

namespace Valhalla_v3.Client.Pages.Jobs;

public partial class JobList
{
    private List<Job> _items = new()
    {
        new(){ Name = "test1", Term = DateTime.Today },
        new(){ Name = "test2", Term = DateTime.Today.AddDays(-1) }
    };
    private bool IsCreateOpen = false;
    private List<Project> projects = new();
    private int projectsId = new();

    protected override async Task OnInitializedAsync()
    {
        LoadJob();
        LoadProject();
    }

    private async Task LoadJob()
    {
        //_items.Clear();
        try
        {
            var response = await Http.GetFromJsonAsync<List<Job>>(navigation.ToAbsoluteUri($"api/job"));
            if (response != null)
            {
                _items.AddRange(response);
                _items = _items.Where(x => x.ProjectId == projectsId).ToList();
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task LoadProject()
    {
        //_items.Clear();
        try
        {
            var response = await Http.GetFromJsonAsync<List<Project>>(navigation.ToAbsoluteUri($"api/project"));
            if (response != null)
            {
                projects = response;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void ItemUpdated(MudItemDropInfo<Job> dropItem)
    {
        dropItem.Item.Term = DateTime.Now;
    }

    private void Create(Job job)
    {

    }
    private void CloseModal()
    {
        IsCreateOpen = false;
    }
    private void OpemModal()
    {
        IsCreateOpen = true;
    }
}
