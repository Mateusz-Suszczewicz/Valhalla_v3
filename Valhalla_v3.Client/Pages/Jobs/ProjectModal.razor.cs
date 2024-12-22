using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Valhalla_v3.Shared.CarHistory;
using Valhalla_v3.Shared.ToDo;

namespace Valhalla_v3.Client.Pages.Jobs;

public partial class ProjectModal
{
    [Parameter]
    public Project project { get; set; } = new();

    [Parameter]
    public EventCallback<Project> ProjctSubmit { get; set; }


    protected override async Task OnInitializedAsync()
    {

    }

    // Obsługa walidacji formularza i wywołanie callbacku
    private async Task HandleValidProjectSubmit()
    {
        await ProjctSubmit.InvokeAsync(project);
        project = new();
    }

}
