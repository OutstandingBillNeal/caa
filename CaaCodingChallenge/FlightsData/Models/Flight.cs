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
    [Required]
    public required string FlightNumber { get; set; }
    [Required]
    public required string Airline { get; set; }
    [Required]
    public required string DepartureAirport { get; set; }
    [Required]
    public required string ArrivalAirport { get; set; }
    [Required]
    public required DateTimeOffset DepartureTime { get; set; }
    [Required]
    public required DateTimeOffset ArrivalTime { get; set; }
    [Required]
    public required FlightStatus Status { get; set; }
}
