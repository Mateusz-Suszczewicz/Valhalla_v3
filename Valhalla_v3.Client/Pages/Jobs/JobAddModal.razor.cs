﻿using Azure.Core.GeoJson;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Valhalla_v3.Shared.CarHistory;
using Valhalla_v3.Shared.ToDo;
using static MudBlazor.CategoryTypes;
using static System.Net.WebRequestMethods;

namespace Valhalla_v3.Client.Pages.Jobs;

public partial class JobAddModal
{
    [Parameter]
    public Job formModel { get; set; }
    [Parameter]
    public List<Project> projects { get; set; }
    private int projectsId = new();
    private string kom { get; set; }
    [Parameter]
    public EventCallback<Job> OnFormSubmit { get; set; }
    [Parameter]
    public EventCallback<bool> OnClose { get; set; }
    [Parameter]
    public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
    }

    // Obsługa walidacji formularza i wywołanie callbacku
    private async Task HandleValidStationSubmit()
    {
        await OnFormSubmit.InvokeAsync(formModel);
        formModel = new Job(); // Czyszczenie danych


    }
    private async Task Close()
    {
        formModel = new Job(); // Czyszczenie danych
        await OnClose.InvokeAsync(false); // Informacja o zamknięciu
    }

    private void addCom()
    {
        Comment com = new()
        {
            OperatorCreateId = 1,
            OperatorModifyId = 1,
            Content = kom,
            JobId = formModel.Id
        };
        formModel.Comments.Add(com);
        kom = "";

    }
}
