using BootstrapBlazor.Components;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Client.Pages;

public partial class Home
{
    private bool isFuelOpen = false;
    private bool isCarOpen = false;
    private List<Car> cars = new List<Car>();

    private int CarId = 0;
    async Task OpenCar()
    {
        cars.Clear();
        try
        {
            var response = await Http.GetFromJsonAsync<List<Car>>(navigation.ToAbsoluteUri("api/car"));
            if (response != null)
            {
                cars.AddRange(response);
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
        }
        isCarOpen = true;
        StateHasChanged();
    }

    void CloseFuel()
    {
        isCarOpen = false;
        isFuelOpen = false;
        StateHasChanged();
    }

    private async void HandleValidStationSubmit(bool choic)
    {
        if (choic)
        {
            System.Console.WriteLine(CarId);

            isCarOpen = false;
            isFuelOpen = true;
        }
        else
        {
            isCarOpen = false;
            isFuelOpen = false;
        }
    }
    private async void HandleFuelSubmit(CarHistoryFuel model)
    {
        model.CarId = CarId;
        model.OperatorCreateId = 1;
        model.OperatorModifyId = 1;
        var json = JsonSerializer.Serialize(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await Http.PostAsync(navigation.ToAbsoluteUri($"api/Fuel"), content);
            if (response.IsSuccessStatusCode)
            {
                CloseFuel();
                model = new();
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
        }

    }
}
