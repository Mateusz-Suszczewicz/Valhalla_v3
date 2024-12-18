using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Valhalla_v3.Client.Helpers;
using Valhalla_v3.Shared;
using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Client.Pages.Cars;

public partial class Details
{
    [Parameter]
    public string Id { get; set; }

    private Tabs activeTab = Tabs.Details;
    private Car? car { get; set; }
    private int mileage { get; set; }
    private decimal FuelCost { get; set; }
    private decimal RepairCost { get; set; }
    private decimal SumCost { get; set; }
    private bool isFuelOpen = false;
    private bool isRapairOpen = false;
    private bool isChoiceOpen = false;
    private bool IsDisabled = true;

    private LineChartOptions lineChartOptions = default!;
    
    private LineChart lineChart1 = default!;
    private LineChart lineChart2 = default!;
    private LineChart lineChart3 = default!;
    private LineChart lineChart4 = default!;
    private LineChart lineChart5 = default!;
    
    private ChartData chartData1 = default!;
    private ChartData chartData2 = default!;
    private ChartData chartData3 = default!;
    private ChartData chartData4 = default!;
    private ChartData chartData5 = default!;

    private void reload()
    {
        List<string> label = new List<string>()
        {
            "Styczeń",
            "Luty",
            "Marzec",
            "Kwieceń",
            "Maj",
            "Czerwiec",
            "Lipiec",
            "Sierpień",
            "Wrzesień",
            "Październik",
            "Listopad",
            "Grudzień"
        };
        var color1 = ColorUtility.CategoricalTwelveColors[1].ToColor();
        var color2 = ColorUtility.CategoricalTwelveColors[2].ToColor();
        var color3 = ColorUtility.CategoricalTwelveColors[3].ToColor();
        var color4 = ColorUtility.CategoricalTwelveColors[4].ToColor();
        var color5 = ColorUtility.CategoricalTwelveColors[5].ToColor();
        lineChartOptions = new() { Responsive = true, Interaction = new Interaction { Mode = InteractionMode.Index } };

        var datasets1 = new List<IChartDataset>() {
        new LineChartDataset
        {
            Label = $"Litry paliwa",
            Data = CarHelpers.GetFuels(car),
            BackgroundColor = color1.ToRgbString(),
            BorderColor = color1.ToRgbString(),
            BorderWidth = 2,
            HoverBorderWidth = 4,
        }};
        chartData1 = new ChartData { Labels = label, Datasets = datasets1 };

        var datasets2 = new List<IChartDataset>() {
        new LineChartDataset
        {
            Label = $"Koszt paliwa",
            Data = CarHelpers.GetCostFuels(car),
            BackgroundColor = color2.ToRgbString(),
            BorderColor = color2.ToRgbString(),
            BorderWidth = 2,
            HoverBorderWidth = 4,
        }};
        chartData2 = new ChartData { Labels = label, Datasets = datasets2 };

        var datasets3 = new List<IChartDataset>() {
        new LineChartDataset
        {
            Label = $"Średnie spalanie",
            Data = CarHelpers.GetAvgBurning(car),
            BackgroundColor = color3.ToRgbString(),
            BorderColor = color3.ToRgbString(),
            BorderWidth = 2,
            HoverBorderWidth = 4,
        }};
        chartData3 = new ChartData { Labels = label, Datasets = datasets3 };

        var datasets4 = new List<IChartDataset>() {
        new LineChartDataset
        {
            Label = $"Przebieg",
            Data = CarHelpers.GetMileage(car),
            BackgroundColor = color4.ToRgbString(),
            BorderColor = color4.ToRgbString(),
            BorderWidth = 2,
            HoverBorderWidth = 4,
        }};
        chartData4 = new ChartData { Labels = label, Datasets = datasets4 };

        var datasets5 = new List<IChartDataset>() {
        new LineChartDataset
        {
            Label = $"Koszty naprawy",
            Data = CarHelpers.GetCostRapair(car),
            BackgroundColor = color5.ToRgbString(),
            BorderColor = color5.ToRgbString(),
            BorderWidth = 2,
            HoverBorderWidth = 4,
        }};
        chartData5 = new ChartData { Labels = label, Datasets = datasets5 };
    }

   

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (lineChart1 != null)
        {
            await lineChart1.InitializeAsync(chartData1, lineChartOptions);
            await lineChart2.InitializeAsync(chartData2, lineChartOptions);
            await lineChart3.InitializeAsync(chartData3, lineChartOptions);
            await lineChart4.InitializeAsync(chartData4, lineChartOptions);
            await lineChart5.InitializeAsync(chartData5, lineChartOptions);
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task SelectTab(Tabs tab)
    {
        activeTab = tab;
        await InvokeAsync(StateHasChanged);
    }

    private async Task LoadCar()
    {
        try
        {
            var response = await Http.GetFromJsonAsync<Car>(navigation.ToAbsoluteUri($"api/car/{Id}"));
            if (response != null)
            {
                car = response;
                ReloadDate();
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    private void ReloadDate()
    {
        FuelCost = car.Fuels.Where(x => x.DateTimeModify.Month == DateTime.Now.Month && x.DateTimeModify.Year == DateTime.Now.Year).Sum(x => x.Cost);
        RepairCost = car.CarHistoryRepair.Where(x => x.Date.Month == DateTime.Now.Month && x.Date.Year == DateTime.Now.Year).Sum(x => x.Cost);
        SumCost = FuelCost + RepairCost;
    }
    
    void OpenFuel()
    {
        isFuelOpen = true;
        StateHasChanged();
    }

    void CloseFuel()
    {
        isFuelOpen = false;
        StateHasChanged();
    }

    private async void HandleFuelSubmit(CarHistoryFuel model)
    {
        model.CarId = car.Id;
        model.OperatorCreateId = 1;
        model.OperatorModifyId = 1;
        var json = JsonSerializer.Serialize(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await Http.PostAsync(navigation.ToAbsoluteUri($"api/Fuel"), content);
            if (response.IsSuccessStatusCode)
            {
                ReloadDate();
                CloseFuel();

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    void OpenRepair()
    {
        isRapairOpen = true;
        StateHasChanged();
    }

    void CloseRepair()
    {
        isRapairOpen = false;
        StateHasChanged();
    }

    private async void HandleRepairSubmit(CarHistoryRepair model)
    {
        model.CarId = car.Id;
        model.OperatorCreateId = 1;
        model.OperatorModifyId = 1;
        var json = JsonSerializer.Serialize(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await Http.PostAsync(navigation.ToAbsoluteUri($"api/Repair"), content);
            if (response.IsSuccessStatusCode)
            {
                ReloadDate();
                CloseFuel();

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        CloseRepair();
    }

    private void EditModal()
    {
        IsDisabled = !IsDisabled;
    }
    
    private void ChoiceModal()
    {
        isChoiceOpen = true;
    }
    
    private async void Delete(bool choic)
    {
        if (!choic)
        {
            isChoiceOpen = false;
            return;
        }
        try
        {
            var response = await Http.DeleteAsync(navigation.ToAbsoluteUri($"api/Car/{car.Id}"));
            if (response.IsSuccessStatusCode)
            {
                navigation.NavigateTo("/cars");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task Save()
    {
        var json = JsonSerializer.Serialize(car);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await Http.PostAsync(navigation.ToAbsoluteUri($"api/Car"), content);
            if (response.IsSuccessStatusCode)
            {
                var Id = response.Content.ReadFromJsonAsync<int>();
                navigation.NavigateTo($"cars/{Id.Result}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

public enum Tabs
{
    Details,
    Statystic
}
