using Valhalla_v3.Shared.CarHistory;

namespace Valhalla_v3.Client.Helpers;

public static class CarHelpers
{
    public static List<double?> GetFuels(Car car)
    {
        List<double?> liters = new List<double?>();
        for (int i = 1; i <= 12; i++)
        {
            liters.Add(car.Fuels.Where(x => x.DateTimeModify.Month == i && x.DateTimeModify.Year == DateTime.Now.Year).Sum(z => Convert.ToDouble(Math.Round(z.Cost / z.CostPerLitr,2))));
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
                var minMilage = car.Fuels?.Where(x => x.DateTimeModify.Month == i && x.DateTimeModify.Year == DateTime.Now.Year).Min(z => z.Mileage);
                var maxMilage = car.Fuels?.Where(x => x.DateTimeModify.Month == i + 1 && x.DateTimeModify.Year == DateTime.Now.Year).Select(z => z.Mileage).FirstOrDefault();
                if(maxMilage ==  null || maxMilage == 0)
                    maxMilage = car.Fuels?.Where(x => x.DateTimeModify.Month == i && x.DateTimeModify.Year == DateTime.Now.Year).Max(z => z.Mileage);
                var pastMilage = maxMilage - minMilage <= 0 ? 1 : maxMilage - minMilage;
                var spalanie = Math.Round(litr / (Convert.ToDecimal(pastMilage) / 100),2);
                liters.Add(Convert.ToDouble(spalanie));
            }
            else
                liters.Add(0);
        }
        return liters;
    }

    public static List<double?> GetMileage(Car car)
    {
        var currentYear = DateTime.Now.Year;
        List<double?> mileage = new List<double?>(12);

        for (int month = 1; month <= 12; month++)
        {
            // Najwyższy przebieg z listy Fuel
            var mileageFuels = car.Fuels?
                .Where(x => x.DateTimeModify.Year == currentYear && x.DateTimeModify.Month == month)
                .Select(z => (double?)Convert.ToDouble(z.Mileage))
                .DefaultIfEmpty(0)
                .Max();

            // Najwyższy przebieg z listy Repair
            var mileageRepair = car.CarHistoryRepair?
                .Where(x => x.DateTimeModify.Year == currentYear && x.DateTimeModify.Month == month)
                .Select(z => (double?)Convert.ToDouble(z.Mileage))
                .DefaultIfEmpty(0)
                .Max();

            // Dodaj wyższy przebieg lub null, jeśli oba są null
            mileage.Add((mileageFuels > mileageRepair) ? mileageFuels : mileageRepair);
        }

        return mileage;
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
