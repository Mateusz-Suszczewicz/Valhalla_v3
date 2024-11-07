using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Valhalla_v3.Shared.CarHistory;
using Microsoft.AspNetCore.SignalR.Client;
using Valhalla_v3.Shared.CarHistory;
using static System.Net.WebRequestMethods;
using System.Net.Http.Json;

namespace Valhalla_v3.Client.Pages.Cars;

public partial class RapairAddModel
{
    private CarHistoryRepair formModel = new CarHistoryRepair();
    private List<Mechanic> ListMechanic = new();

    [Parameter]
    public EventCallback<CarHistoryRepair> OnFormSubmit { get; set; }


    protected override async Task OnInitializedAsync()
    {
        await LoadMechnic();
    }

    private async Task LoadMechnic()
    {
        try
        {
            var response = await Http.GetFromJsonAsync<List<Mechanic>>(navigation.ToAbsoluteUri($"api/Mechanic"));
            if (response != null)
            {
                ListMechanic = response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    // Obsługa walidacji formularza i wywołanie callbacku
    private async Task HandleValidSubmit()
    {
        await OnFormSubmit.InvokeAsync(formModel);
    }

}
