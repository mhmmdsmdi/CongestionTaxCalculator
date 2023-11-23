using CongestionTaxCalculator.Models;
using CongestionTaxCalculator.Services;
using Microsoft.AspNetCore.Mvc;

namespace CongestionTaxCalculator.Controllers;

[ApiController]
public class HomeController(ILogger<HomeController> logger,
        ICongestionTaxCalculatorService congestionTaxCalculatorService, IVehicle vehicle)
    : ControllerBase
{
    [Route("")]
    [HttpPost]
    public int GetTax(List<DateTime> dates)
    {
        return congestionTaxCalculatorService.GetTax(vehicle, dates.ToArray());
    }
}