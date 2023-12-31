using CongestionTaxCalculator.Contexts;
using CongestionTaxCalculator.Models;

namespace CongestionTaxCalculator.Services;

public interface ICongestionTaxCalculatorService
{
    /**
 * Calculate the total toll fee for one day
 *
 * @param vehicle - the vehicle
 * @param dates   - date and time of all passes on one day
 * @return - the total congestion tax for that day
 */

    int GetTax(IVehicle vehicle, DateTime[] dates);
}

public class CongestionTaxCalculatorService : ICongestionTaxCalculatorService
{
    private readonly ApplicationDbContext _dbContext;

    public CongestionTaxCalculatorService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /**
 * Calculate the total toll fee for one day
 *
 * @param vehicle - the vehicle
 * @param dates   - date and time of all passes on one day
 * @return - the total congestion tax for that day
 */

    public int GetTax(IVehicle vehicle, DateTime[] dates)
    {
        var intervalStart = dates[0];
        var totalFee = 0;
        foreach (var date in dates)
        {
            var nextFee = GetTollFee(date, vehicle);
            var tempFee = GetTollFee(intervalStart, vehicle);

            long diffInMillies = date.Millisecond - intervalStart.Millisecond;
            var minutes = diffInMillies / 1000 / 60;

            if (minutes <= 60)
            {
                if (totalFee > 0) totalFee -= tempFee;
                if (nextFee >= tempFee) tempFee = nextFee;
                totalFee += tempFee;
            }
            else
            {
                totalFee += nextFee;
            }
        }

        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    private bool IsTollFreeVehicle(IVehicle vehicle)
    {
        if (vehicle == null) return false;
        var vehicleType = vehicle.GetVehicleType();
        return vehicleType.Equals(TollFreeVehicles.Motorcycle.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Military.ToString());
    }

    public int GetTollFee(DateTime date, IVehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        var hour = date.Hour;
        var minute = date.Minute;
        var time = hour.ToString("D2") + ":" + minute.ToString("D2");

        var fee = _dbContext.Fees
            .FirstOrDefault(hf => time.CompareTo(hf.StartHour) >= 0 &&
                                  time.CompareTo(hf.EndHour) <= 0);

        return fee?.Fee ?? 0;
    }

    private bool IsTollFreeDate(DateTime date)
    {
        var year = date.Year;
        var month = date.Month;
        var day = date.Day;

        if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) return true;

        if (year != 2013) return false;
        return (month == 1 && day == 1) ||
               (month == 3 && day is 28 or 29) ||
               (month == 4 && day is 1 or 30) ||
               (month == 5 && day is 1 or 8 or 9) ||
               (month == 6 && day is 5 or 6 or 21) ||
               month == 7 ||
               (month == 11 && day == 1) ||
               (month == 12 && day is 24 or 25 or 26 or 31);
    }

    private enum TollFreeVehicles
    {
        Motorcycle = 0,
        Tractor = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5
    }
}