using CongestionTaxCalculator.Models;
using CongestionTaxCalculator.Services;
using Microsoft.AspNetCore.Mvc;

namespace CongestionTaxCalculator.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICongestionTaxCalculatorService _congestionTaxCalculatorService;
    private readonly IVehicle _vehicle;

    public HomeController(ILogger<HomeController> logger, ICongestionTaxCalculatorService congestionTaxCalculatorService, IVehicle vehicle)
    {
        _logger = logger;
        _congestionTaxCalculatorService = congestionTaxCalculatorService;
        _vehicle = vehicle;
    }

    [HttpPost()]
    public int GetTax([FromQuery] DateTime[] dates)
    {
        return _congestionTaxCalculatorService.GetTax(_vehicle, dates);
    }
}