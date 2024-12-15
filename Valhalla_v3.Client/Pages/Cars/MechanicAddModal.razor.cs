using Microsoft.AspNetCore.Components;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Client.Pages.Cars;

public partial class MechanicAddModal
{
    private Mechanic formModel = new Mechanic();

    [Parameter]
    public EventCallback<Mechanic> OnFormStationSubmit { get; set; }

    protected override async Task OnInitializedAsync()
    {
    }

    // Obsługa walidacji formularza i wywołanie callbacku
    private async Task HandleValidStationSubmit()
    {
        await OnFormStationSubmit.InvokeAsync(formModel);
        formModel = new();
    }

}