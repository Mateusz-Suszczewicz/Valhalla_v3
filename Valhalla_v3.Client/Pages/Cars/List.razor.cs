using System.Net.Http.Json;
using Valhalla_v3.Shared.CarHistory;
using static MudBlazor.CategoryTypes;
using Valhalla_v3.Shared.ToDo;

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
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    messages = await response.Content.ReadFromJsonAsync<List<Car>>() ?? new List<Car>();
                    ErrorMessage = string.Empty;
                    break;

                case System.Net.HttpStatusCode.NoContent:
                    return;

                default:
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    ErrorMessage = $"Błąd API - {response.StatusCode}: {errorDetails}";
                    break;
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
