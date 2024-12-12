using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using System;
using System.Drawing;
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
    public string Id { get; set; } = string.Empty;

    private Tabs activeTab = Tabs.Details;
    private Car? car;
    private int mileage;
    private decimal fuelCost;
    private decimal repairCost;
    private decimal sumCost;
    private bool isFuelOpen;
    private bool isRepairOpen;
    private bool isChoiceOpen;
    private bool isDisabled = true;

    private LineChartOptions lineChartOptions = new();
    private ChartData[] chartData = new ChartData[5];
    private LineChart[] lineCharts = new LineChart[5];

    protected override async Task OnInitializedAsync()
    {
        car = new Car { OperatorCreateId = 1, OperatorModifyId = 1 };

        if (Id != "0")
        {
            await LoadCarAsync();
        }
        else
        {
            isDisabled = false;
        }

        ReloadCharts();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            ReloadCharts();
            for (int i = 0; i < lineCharts.Length; i++)
            {
                if (lineCharts[i] != null)
                {
                    await lineCharts[i].InitializeAsync(chartData[i], lineChartOptions);
                }
            }
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private void ReloadCharts()
    {
        var labels = new[]
        {
            "Styczeń", "Luty", "Marzec", "Kwieceń", "Maj", "Czerwiec",
            "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień"
        };

        var colors = ColorUtility.CategoricalTwelveColors;

        chartData[0] = CreateChartData("Litry paliwa", CarHelpers.GetFuels(car), colors[1].ToColor());
        chartData[1] = CreateChartData("Koszt paliwa", CarHelpers.GetCostFuels(car), colors[2].ToColor());
        chartData[2] = CreateChartData("Średnie spalanie", CarHelpers.GetAvgBurning(car), colors[3].ToColor());
        chartData[3] = CreateChartData("Przebieg", CarHelpers.GetMileage(car), colors[4].ToColor());
        chartData[4] = CreateChartData("Koszty naprawy", CarHelpers.GetCostRapair(car), colors[5].ToColor());
    }

    private static ChartData CreateChartData(string label, IEnumerable<double?> data, Color color)
    {
        return new ChartData
        {
            Labels = new List<string>
            {
                "Styczeń", "Luty", "Marzec", "Kwieceń", "Maj", "Czerwiec",
                "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień"
            },
            Datasets = new List<IChartDataset>
            {
                new LineChartDataset
                {
                    Label = label,
                    Data = data.ToList(),
                    BackgroundColor = color.ToRgbString(),
                    BorderColor = color.ToRgbString(),
                    BorderWidth = 2,
                    HoverBorderWidth = 4
                }
            }
        };
    }

    private async Task LoadCarAsync()
    {
        try
        {
            car = await Http.GetFromJsonAsync<Car>(navigation.ToAbsoluteUri($"api/car/{Id}"));
            if (car != null)
            {
                ReloadCosts();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void ReloadCosts()
    {
        if (car != null)
        {
            var now = DateTime.Now;
            fuelCost = car.Fuels.Where(f => f.DateTimeModify.Month == now.Month && f.DateTimeModify.Year == now.Year).Sum(f => f.Cost);
            repairCost = car.CarHistoryRepair.Where(r => r.Date.Month == now.Month && r.Date.Year == now.Year).Sum(r => r.Cost);
            sumCost = fuelCost + repairCost;
        }
    }

    private async Task HandleSubmitAsync<T>(string apiEndpoint, T model)
    {
        model.GetType().GetProperty("CarId")?.SetValue(model, car?.Id);
        model.GetType().GetProperty("OperatorCreateId")?.SetValue(model, 1);
        model.GetType().GetProperty("OperatorModifyId")?.SetValue(model, 1);

        try
        {
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await Http.PostAsync(navigation.ToAbsoluteUri(apiEndpoint), content);
            if (response.IsSuccessStatusCode)
            {
                ReloadCosts();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task DeleteAsync(bool confirm)
    {
        if (!confirm) return;

        try
        {
            var response = await Http.DeleteAsync(navigation.ToAbsoluteUri($"api/Car/{car?.Id}"));
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

    private async Task SelectTab(Tabs tab)
    {
        if (activeTab != tab)
        {
            activeTab = tab;
            await InvokeAsync(StateHasChanged);
        }
    }

    private void ToggleModal(ref bool modalState, bool newState)
    {
        modalState = newState;
        StateHasChanged();
    }

    private void OpenFuel() => ToggleModal(ref isFuelOpen, true);

    private void CloseFuel() => ToggleModal(ref isFuelOpen, false);

    private void OpenRepair() => ToggleModal(ref isRepairOpen, true);

    private void CloseRepair() => ToggleModal(ref isRepairOpen, false);

    private void ChoiceModal() => ToggleModal(ref isChoiceOpen, true);

    private void ToggleState(ref bool state)
    {
        state = !state;
        StateHasChanged();
    }

    private void EditModal() => ToggleState(ref isDisabled);

    private async Task SaveAsync()
    {
        var json = JsonSerializer.Serialize(car);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await Http.PostAsync(navigation.ToAbsoluteUri($"api/Car"), content);
            if (response.IsSuccessStatusCode)
            {
                var Id = await response.Content.ReadFromJsonAsync<int>();
                navigation.NavigateTo($"cars/{Id}");
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
