using FlightsData.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightsData
{
    public interface IFlightsContext
    {
        string DbPath { get; }
        DbSet<Flight> Flights { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}