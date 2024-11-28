using Microsoft.AspNetCore.Components;
using Valhalla_v3.Shared.CarHistory;
using Valhalla_v3.Shared.ToDo;

namespace Valhalla_v3.Client.Pages.Jobs;

public partial class JobAddModal
{
    private Job formModel = new Job();
    private List<Project> projects = new();
    private int projectsId = new();
    private string kom { get; set; }
    [Parameter]
    public EventCallback<Job> OnFormSubmit { get; set; }


    protected override async Task OnInitializedAsync()
    {
        var com = new Comment() { Content = "lorem impousum" };
        formModel.Comments.Add(com);
    }

    // Obsługa walidacji formularza i wywołanie callbacku
    private async Task HandleValidStationSubmit()
    {
        await OnFormSubmit.InvokeAsync(formModel);
    }

}
