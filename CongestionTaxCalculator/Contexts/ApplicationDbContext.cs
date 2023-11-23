using CongestionTaxCalculator.Models;
using Microsoft.EntityFrameworkCore;

namespace CongestionTaxCalculator.Contexts;

public class ApplicationDbContext : DbContext
{
    public DbSet<HourFee> Fees { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}