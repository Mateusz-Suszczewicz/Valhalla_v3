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
            if (car.Fuels.Any(x => x.DateTimeModify.Month == i && x.DateTimeModify.Year == DateTime.Now.Year))
            {
                var litr = car.Fuels.Where(x => x.DateTimeModify.Month == i && x.DateTimeModify.Year == DateTime.Now.Year).Sum(z => z.Cost / z.CostPerLitr);
                litr -= car.Fuels.Where(x => x.DateTimeModify.Month == i && x.DateTimeModify.Year == DateTime.Now.Year).Select(z => z.Cost / z.CostPerLitr).FirstOrDefault();
                litr += car.Fuels.Where(x => x.DateTimeModify.Month == i + 1 && x.DateTimeModify.Year == DateTime.Now.Year).Select(z => z.Cost / z.CostPerLitr).FirstOrDefault();
                if (i == 12)
                    litr += car.Fuels.Where(x => x.DateTimeModify.Month == 1 && x.DateTimeModify.Year == DateTime.Now.Year + 1).Select(z => z.Cost / z.CostPerLitr).FirstOrDefault();
                var minMilage = car.Fuels?.Where(x => x.DateTimeModify.Month == i && x.DateTimeModify.Year == DateTime.Now.Year)?.Min(z => z.Mileage);
                var maxMilage = car.Fuels?.Where(x => x.DateTimeModify.Month == i + 1 && x.DateTimeModify.Year == DateTime.Now.Year).Select(z => z.Mileage).FirstOrDefault();
                var pastMilage = maxMilage - minMilage <= 0 ? 1 : maxMilage - minMilage;
                var spalanie = litr / (Convert.ToDecimal(pastMilage) / 100);
                liters.Add(Convert.ToDouble(spalanie));
            }
            else
                liters.Add(0);
        }
        return liters;
    }

    public static List<double?> GetMileage(Car car)
    {
        List<double?> liters = new List<double?>();
        for (int i = 1; i <= 12; i++)
        {
            var milage = car.Fuels.Where(x => x.DateTimeModify.Month == i && x.DateTimeModify.Year == DateTime.Now.Year).Sum(z => Convert.ToDouble(z.Mileage));
            milage += car.CarHistoryRepair.Where(x => x.DateTimeModify.Month == i && x.DateTimeModify.Year == DateTime.Now.Year).Sum(z => Convert.ToDouble(z.Mileage));
            liters.Add(milage);
        }
        return liters;
    }

    public static List<double?> GetCostRapair(Car car)
    {
        List<double?> liters = new List<double?>();
        for (int i = 1; i <= 12; i++)
        {
            liters.Add(car.CarHistoryRepair.Where(x => x.DateTimeModify.Month == i && x.DateTimeModify.Year == DateTime.Now.Year).Sum(z => Convert.ToDouble(z.Cost)));
        }
        return liters;
    }
}
