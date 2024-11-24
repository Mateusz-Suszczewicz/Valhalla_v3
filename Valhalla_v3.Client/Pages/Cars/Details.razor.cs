using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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
    private bool isFuelOpen = false;
    private bool isRapairOpen = false;
    private bool isChoiceOpen = false;
    private bool IsDisabled = true;

    private LineChart lineChart = default!;
    private LineChartOptions lineChartOptions = default!;
    private ChartData chartData = default!;
    private int datasetsCount;
    private int labelsCount;

    private Random random = new();
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
        var c = ColorUtility.CategoricalTwelveColors[datasetsCount].ToColor();

        var datasets = new List<IChartDataset>() {
        new LineChartDataset
        {
            Label = $"Litry paliwa",
            Data = GetCost(),
            BackgroundColor = c.ToRgbString(),
            BorderColor = c.ToRgbString(),
            BorderWidth = 2,
            HoverBorderWidth = 4,
            // PointBackgroundColor = c.ToRgbString(),
            // PointRadius = 0, // hide points
            // PointHoverRadius = 4,
        }}
        ;
        chartData = new ChartData { Labels = label, Datasets = datasets };
        lineChartOptions = new() { Responsive = true, Interaction = new Interaction { Mode = InteractionMode.Dataset } };
        lineChartOptions.Scales.Y!.Max = car.Fuels.Max(x => Convert.ToDouble(x.Cost/x.CostPerLitr))*1.1;
    }
    protected override async Task OnInitializedAsync()
    {
        Operator oper = new() { Name = "admin", Id = 3 };
        car = new() { OperatorCreate = oper, OperatorModify = oper };
        await LoadCar();
        reload();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (lineChart != null)
        {
            await lineChart.InitializeAsync(chartData, lineChartOptions);
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
    }

    private List<double?> GetCost()
    {
        var liters = new List<double?>();
        for (int i = 1; i <= 12; i++)
        {
            liters.Add(car.Fuels.Where(x => x.DateTimeModify.Month == i && x.DateTimeModify.Year == DateTime.Now.Year).Sum(z => Convert.ToDouble(z.Cost / z.CostPerLitr)));
        }
        return liters;
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
        model.OperatorCreateId = 3;
        model.OperatorModifyId = 3;
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
        model.OperatorCreateId = 3;
        model.OperatorModifyId = 3;
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
                navigation.NavigateTo("/Car");
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
                IsDisabled = true;
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
