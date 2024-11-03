using Microsoft.AspNetCore.SignalR.Client;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Client.Pages.Cars;

public partial class List
{
    private List<Car> messages = new();

    protected override async Task OnInitializedAsync()
    {
        //TODO: Pobranie listy aut. 
    }
}
