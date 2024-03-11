using FlyZenService.Infrastructure.Moq;
using FlyZenService.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var tickets = new List<Ticket>();

app.MapGet("/tickets", ([FromQuery] DateTime from, [FromQuery] DateTime to) =>
{

    if (to < from) return Results.BadRequest(new Error("400", "Date 'To' letter than Date 'from' "));
    var days = (to - from).TotalDays;
    tickets.Clear();

    for (int i = 0; i < days; i++)
    {
        tickets.AddRange(TicketGenerator.Generate(new Random().Next(30, 60), from));
    }

    return Results.Ok(tickets);

})
.WithName("tickets")
.WithOpenApi();

app.MapPost("/tickets/{ticket}/order", ([FromRoute(Name = "ticket")] string ticketNumber) =>
{
    var ticket = tickets.FirstOrDefault(x => x.Number.Equals(ticketNumber));

    if (ticket == null) return Results.BadRequest(new Error("404", "ticket not found"));

    if (ticket.SeatingCount == 0) return Results.BadRequest(new Error("400", "There are no available seats on this flight"));

    ticket.SeatingCount--;

    return Results.Ok();
}).WithName("order")
.WithOpenApi();

app.Run();

