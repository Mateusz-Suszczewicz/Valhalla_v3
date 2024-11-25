using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Client.Helpers;

public static class CarHelpers
{
    public static List<double?> GetFuels(Car car)
    {
        List<double?> liters = new List<double?>();
        for (int i = 1; i <= 12; i++)
        {
            liters.Add(car.Fuels.Where(x => x.DateTimeModify.Month == i && x.DateTimeModify.Year == DateTime.Now.Year).Sum(z => Convert.ToDouble(z.Cost / z.CostPerLitr)));
        }
        return liters;
    }

    public static List<double?> GetCostFuels(Car car)
    {
        List<double?> liters = new List<double?>();
        for (int i = 1; i <= 12; i++)
        {
            liters.Add(car.Fuels.Where(x => x.DateTimeModify.Month == i && x.DateTimeModify.Year == DateTime.Now.Year).Sum(z => Convert.ToDouble(z.Cost)));
        }
        return liters;
    }

    public static List<double?> GetAvgBurning(Car car)
    {
        List<double?> liters = new List<double?>();
        for (int i = 1; i <= 12; i++)
        {
            liters.Add(car.Fuels.Where(x => x.DateTimeModify.Month == i && x.DateTimeModify.Year == DateTime.Now.Year).Sum(z => Convert.ToDouble(z.Cost)));
        }
        return liters;
    }

    public static List<double?> GetMileage(Car car)
    {
        List<double?> liters = new List<double?>();
        for (int i = 1; i <= 12; i++)
        {
            liters.Add(car.Fuels.Where(x => x.DateTimeModify.Month == i && x.DateTimeModify.Year == DateTime.Now.Year).Sum(z => Convert.ToDouble(z.Cost)));
        }
        return liters;
    }

    public static List<double?> GetCostRapair(Car car)
    {
        List<double?> liters = new List<double?>();
        for (int i = 1; i <= 12; i++)
        {
            liters.Add(car.Fuels.Where(x => x.DateTimeModify.Month == i && x.DateTimeModify.Year == DateTime.Now.Year).Sum(z => Convert.ToDouble(z.Cost)));
        }
        return liters;
    }
}
