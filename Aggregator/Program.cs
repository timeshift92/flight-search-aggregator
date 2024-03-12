using ActualLab.Rpc.Server;
using Aggregator.Data;
using Aggregator.HostedServices;
using Aggregator.HostedServices.FlyZen;
using Aggregator.HostedServices.ZotFlight;
using Aggregator.Infrastructure.ServiceCollection;
using Coravel;
using Microsoft.EntityFrameworkCore;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.SQLite(@"Logs\log.db")
    .ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddTransient<ZotFlightServiceScheduler>();
builder.Services.AddTransient<FlyZenServiceScheduler>();
var cfg = builder.Configuration;
var env = builder.Environment;

#region HttpClients

builder.Services.AddHttpClient("ZotFlightService", (cl) =>
{
    cl.BaseAddress = new Uri(cfg.GetValue<string>("ZotFlightService:Url")!);
    cl.DefaultRequestHeaders.Add("apiToken", cfg.GetValue<string>("ZotFlightService:Token")!);
});

builder.Services.AddHttpClient("FlyZenService", (cl) =>
{
    cl.BaseAddress = new Uri(cfg.GetValue<string>("FlyZenService:Url")!);
    cl.DefaultRequestHeaders.Add("apiToken", cfg.GetValue<string>("FlyZenService:Token")!);
});

#endregion

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

#region Scheduler

builder.Services.AddScheduler();

#endregion

#region Fusion

builder.Services.AddFusionService();

#endregion

#region DB

builder.Services.AddDataBase<AppDbContext>(env, cfg);

#endregion

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Services.UseScheduler(scheduler =>
{
    scheduler
        .Schedule<ZotFlightServiceScheduler>()
        .Cron("* * * * *")
        .PreventOverlapping("ZotFlightServiceScheduler");
    scheduler
        .Schedule<FlyZenServiceScheduler>()
        .Cron("* * * * *")
        .PreventOverlapping("FlyZenServiceScheduler");
});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapRpcWebSocketServer();
app.MapFallbackToPage("/_Host");

var dbContextFactory = app.Services.GetRequiredService<IDbContextFactory<AppDbContext>>();
using var dbContext = dbContextFactory.CreateDbContext();
//await dbContext.Database.MigrateAsync();
// await dbContext.Database.EnsureDeletedAsync();
dbContext.Database.EnsureCreated();
app.Run();