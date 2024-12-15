using System.Net.Http.Json;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Client.Pages.Cars;

public partial class List
{
    private List<Car> messages = new();
    private string ErrorMessage;

    protected override async Task OnInitializedAsync()
    {
        await LoadCars();
    }

    private async Task LoadCars()
    {
        messages.Clear();
        try
        {
            var response = await Http.GetAsync(navigation.ToAbsoluteUri("api/car"));
            if (response.IsSuccessStatusCode)
            {

                messages = await response.Content.ReadFromJsonAsync<List<Car>>();
                ErrorMessage =  string.Empty;
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                ErrorMessage = $"Błąd API - {response.StatusCode}: {errorDetails}";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Błąd aplikacji: {ex.Message} StackTrace: {ex.StackTrace}";
            Console.WriteLine(ex.Message);
        }
    }

    private void Create()
    {
        navigation.NavigateTo("/cars/0");
    }
}
