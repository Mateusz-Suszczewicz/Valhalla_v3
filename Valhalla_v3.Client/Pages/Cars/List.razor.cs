using System.Net.Http.Json;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Client.Pages.Cars;

public partial class List
{
    private List<Car> messages = new();
    private string error;
    protected override async Task OnInitializedAsync()
    {
        await LoadCars();
    }

    private async Task LoadCars()
    {
        try
        {
            var response = await Http.GetFromJsonAsync<List<Car>>(navigation.ToAbsoluteUri("api/car"));
            if (response != null)
            {
                messages.AddRange(response);
            }
        }
        catch(Exception ex)
        {
            error = ex.Message;
            Console.WriteLine(ex.Message);
        }
    }

    private void Create()
    {
        navigation.NavigateTo("/cars/0");
    }
}
