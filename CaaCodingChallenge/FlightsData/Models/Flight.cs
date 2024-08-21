using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightsData.Models;

[Index(nameof(FlightNumber), IsUnique = true)]
public class Flight
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string FlightNumber { get; set; }
    public required string Airline { get; set; }
    public required string DepartureAirport { get; set; }
    public required string ArrivalAirport { get; set; }
    public required DateTimeOffset DepartureTime { get; set; }
    public required DateTimeOffset ArrivalTime { get; set; }
    public required FlightStatus Status { get; set; }
}
