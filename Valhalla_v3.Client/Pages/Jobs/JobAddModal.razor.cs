using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Valhalla_v3.Shared.CarHistory;
using Valhalla_v3.Shared.ToDo;
using static MudBlazor.CategoryTypes;
using static System.Net.WebRequestMethods;

namespace Valhalla_v3.Client.Pages.Jobs;

public partial class JobAddModal
{
    private Job formModel = new Job();
    private List<Project> projects = new();
    private int projectsId = new();
    private string kom { get; set; }
    [Parameter]
    public EventCallback<Job> OnFormSubmit { get; set; }
    [Parameter]
    public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var response = await Http.GetFromJsonAsync<Job>(navigation.ToAbsoluteUri($"api/job/{Id}"));
            if (response != null)
            {
                formModel = response;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        LoadProject();


    }
    private async Task LoadProject()
    {
        projects.Clear();
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

    // Obsługa walidacji formularza i wywołanie callbacku
    private async Task HandleValidStationSubmit()
    {
        await OnFormSubmit.InvokeAsync(formModel);
    }

}
