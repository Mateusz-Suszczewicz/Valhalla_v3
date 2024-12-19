using Microsoft.AspNetCore.Components;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Client.Pages.Cars;

public partial class MechanicAddModal
{
    [Parameter]
    public Mechanic mechanic { get; set; } = new();

    [Parameter]
    public EventCallback<Mechanic> MechanicSubmit { get; set; }

    protected override async Task OnInitializedAsync()
    {
    }

    // Obsługa walidacji formularza i wywołanie callbacku
    private async Task HandleValidStationSubmit()
    {
        await MechanicSubmit.InvokeAsync(mechanic);
        mechanic = new();
    }

}