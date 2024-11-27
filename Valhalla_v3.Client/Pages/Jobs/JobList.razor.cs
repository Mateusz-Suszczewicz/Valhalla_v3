using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MudBlazor;
using System.Net.Http.Json;
using Valhalla_v3.Shared.CarHistory;
using Valhalla_v3.Shared.ToDo;
using static System.Net.WebRequestMethods;

namespace Valhalla_v3.Client.Pages.Jobs;

public partial class JobList
{
    private List<Job> _items = new()
    {
        new Job(){ Name = "Ogarnąć ikonki", IsCompleted = "false" },
        new Job(){ Name = "Ładowanie zadań", IsCompleted = "true" },
        new Job(){ Name = "Just Mud", IsCompleted = "false" },
    };

    protected override async Task OnInitializedAsync()
    {
        LoadJob();
    }

    private async Task LoadJob()
    {
        try
        {
            var response = await Http.GetFromJsonAsync<List<Job>>(navigation.ToAbsoluteUri($"api/job/"));
            if (response != null)
            {
                _items = response;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void ItemUpdated(MudItemDropInfo<Job> dropItem)
    {
        dropItem.Item.IsCompleted = dropItem.DropzoneIdentifier;
    }

    

}
