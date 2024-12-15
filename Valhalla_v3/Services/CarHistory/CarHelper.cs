namespace Valhalla_v3.Services.CarHistory;

public static class CarHelper
{
    public static bool MileageValidate(int fuelMileage, int repairMileage, int newMileage)
    {
        return newMileage > fuelMileage && newMileage > repairMileage;
    }
}
