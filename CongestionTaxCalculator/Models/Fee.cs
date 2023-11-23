using System.ComponentModel.DataAnnotations;

namespace CongestionTaxCalculator.Models;

public class HourFee
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string StartHour { get; set; }

    [Required]
    public string EndHour { get; set; }

    [Required]
    public int Fee { get; set; }
}