﻿using FlyZenService.Models;

namespace FlyZenService.Infrastructure.Moq;

public static class TicketGenerator
{
    static readonly Random rnd = new();
    public static List<Ticket> Generate(int ticketCount, DateTime from)
    {
        var tickets = new List<Ticket>();

        for (int i = 0; i <= ticketCount; i++)
        {
            tickets.Add(new Ticket(Faker.Identification.SocialSecurityNumber(true), Faker.Company.Name(), GenerateFlights(rnd.Next(1, 5), from), rnd.Next(100, 1000), rnd.Next(5, 60)));
        }
        return tickets;
    }

    static List<Flight> GenerateFlights(int flightCount, DateTime from)
    {
        List<string> cities = ["Tashkent","Samarqand","Nukus","Jizzax","Andijon","Namangan","Fergana","Bukhara","Qarshi","Kokand","Margilan","Termez","Angren","Chirchiq","Navoiy","Urgench","Shahrisabz"];

        var flights = new List<Flight> { new(cities[rnd.Next(0, cities.Count -1)] , from, cities[rnd.Next(0, cities.Count -1)], from.AddHours(1)) };

        for (int i = 1; i < flightCount; i++)
        {
            var prevFlight = flights[i - 1];

            List<int> times = [15, 30, 45, 60];
            var nextDate = prevFlight.ArrivalTime.AddHours(1).AddMinutes(times[rnd.Next(0, 2)]);

            flights.Add(new Flight(
                prevFlight.ArrivalCity, nextDate,
                cities[rnd.Next(0, cities.Count -1)], nextDate.AddMinutes(times[rnd.Next(0, 2)] + 30)
                ));
        }
        return flights;
    }
}
