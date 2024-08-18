using FlightsData.Models;

namespace TestHelpers
{
    public static class Any
    {
        public static string String()
        {
            return Guid.NewGuid().ToString();
        }

        public static int Int()
        {
            return new Random().Next(1, 1000000);
        }

        public static Flight Flight()
        {
            return new Flight
            {
                Id = Int(),
                FlightNumber = String(),
                Airline = String(),
                DepartureAirport = String(),
                ArrivalAirport = String(),
                DepartureTime = DateTimeOffset.Now,
                ArrivalTime = DateTimeOffset.Now,
                Status = FlightStatus.Scheduled
            };
        }
    }
}
