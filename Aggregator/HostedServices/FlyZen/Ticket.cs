namespace FlyZenService.Models
{
    public class Ticket(string Number, string Airline, List<Flight> Flights, decimal Price, int SeatingCount)
    {
        public string Number { get; set; } = Number;
        public string Airline { get; set; } = Airline;
        public List<Flight> Flights { get; set; } = Flights;
        public decimal Price { get; set; } = Price;
        public int SeatingCount { get; set; } = SeatingCount;
    }
}
