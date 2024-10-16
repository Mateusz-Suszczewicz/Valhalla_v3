﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Valhalla_v3.Shared.CarHistory;
using Microsoft.AspNetCore.SignalR.Client;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Client.Pages.Cars;

public partial class RapairAddModel
{
    private CarHistoryRepair formModel = new CarHistoryRepair();
    private List<Mechanic> ListMechanic = new();
    private HubConnection _hubConnection;

    [Parameter]
    public EventCallback<CarHistoryRepair> OnFormSubmit { get; set; }


    protected override async Task OnInitializedAsync()
    {
        _hubConnection = new HubConnectionBuilder()
        .WithUrl(navigation.ToAbsoluteUri("/carhub"))
        .Build();

        _hubConnection.On<List<Mechanic>>("Mechanics", (receivedItem) =>
        {
            ListMechanic = receivedItem;
            InvokeAsync(StateHasChanged);
        });

        await _hubConnection.StartAsync();
        await _hubConnection.InvokeAsync("GetMechanic");
    }

    // Obsługa walidacji formularza i wywołanie callbacku
    private async Task HandleValidSubmit()
    {
        await OnFormSubmit.InvokeAsync(formModel);
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
    }
}
