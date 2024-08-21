using FlightsData.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightsData;

public class FlightsContext
    : DbContext
{
    public DbSet<Flight> Flights { get; set; }

    public string DbPath { get; }

    public FlightsContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "flights.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={DbPath}");
}
