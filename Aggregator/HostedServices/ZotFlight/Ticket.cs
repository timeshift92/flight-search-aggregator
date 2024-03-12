namespace Aggregator.HostedServices.ZotFlight
{
    public class Ticket(string number, string airline, List<Flight> flights, decimal price, int seatingCount)
    {
        public string Number { get; set; } = number;
        public string Airline { get; set; } = airline;
        public List<Flight> Flights { get; set; } = flights;
        public decimal Price { get; set; } = price;
        public int SeatingCount { get; set; } = seatingCount;
    }
}