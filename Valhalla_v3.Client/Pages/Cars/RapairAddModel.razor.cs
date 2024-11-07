﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Valhalla_v3.Shared.CarHistory;
using Microsoft.AspNetCore.SignalR.Client;
using Valhalla_v3.Shared.CarHistory;
using static System.Net.WebRequestMethods;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Valhalla_v3.Client.Pages.Cars;

public partial class RapairAddModel
{
    private CarHistoryRepair formModel = new CarHistoryRepair();
    private List<Mechanic> ListMechanic = new();
    private bool isMechanicOpen = false;
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
    private async Task HandleValidMechanicSubmit()
    {
        await OnFormSubmit.InvokeAsync(formModel);
    }


    void OpenStation()
    {
        isMechanicOpen = true;
        StateHasChanged();
    }

    async Task CloseStation()
    {

        isMechanicOpen = false;
        StateHasChanged();
    }

    private async void HandleMechanicSubmit(Mechanic model)
    {
        //TODO: dodanie mechaników
        model.OperatorCreateId = 3;
        model.OperatorModifyId = 3;
        var json = JsonSerializer.Serialize(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await Http.PostAsync(navigation.ToAbsoluteUri($"api/Mechanic"), content);
            if (response.IsSuccessStatusCode)
            {
                LoadMechnic();
                CloseStation();

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        CloseStation();
    }
}
