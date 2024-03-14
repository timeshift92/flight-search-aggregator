using ActualLab.Rpc.Server;
using Aggregator.Data;
using Aggregator.HostedServices;
using Aggregator.HostedServices.FlyZen;
using Aggregator.HostedServices.ZotFlight;
using Aggregator.Infrastructure.ServiceCollection;
using Coravel;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Serilog;


var builder = WebApplication.CreateBuilder(args);



#region Swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
#endregion

#region Log

var currentDir = Directory.GetCurrentDirectory();

// get connection string from configuration file (appsettings.json)
string? sqliteLoggerConnectionString = builder.Configuration.GetConnectionString("SqliteLogger");
SqliteConnectionStringBuilder sqliteLoggerConnectionStringBuilder =
    new SqliteConnectionStringBuilder(sqliteLoggerConnectionString);
sqliteLoggerConnectionStringBuilder.DataSource =
    Path.Combine(currentDir, sqliteLoggerConnectionStringBuilder.DataSource);
string sqliteDbFilePath = sqliteLoggerConnectionStringBuilder.DataSource;

// file logger path
string serilogFileLoggerFilePath = Path.Combine(currentDir, "Logs", "logs.log");

builder.Host.UseSerilog((ctx, lc) => lc
    .MinimumLevel.Information()
    //.WriteTo.Console(new JsonFormatter(), restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .WriteTo.SQLite(sqliteDbFilePath,
        tableName: "Logs",
        restrictedToMinimumLevel:
        builder.Environment.IsDevelopment()
            ? Serilog.Events.LogEventLevel.Information
            : Serilog.Events.LogEventLevel.Warning,
        storeTimestampInUtc: false,
        batchSize:
        builder.Environment.IsDevelopment() ? (uint)1 : (uint)100,
        retentionPeriod: new TimeSpan(0, 1, 0, 0, 0),
        maxDatabaseSize: 10)
);

#endregion

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

builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddDataBase<AppDbContext>(env, cfg);
builder.Services.AddDbContext<ApplicationLoggerDbContext>(options =>
    options.UseSqlite(sqliteLoggerConnectionStringBuilder.ConnectionString)
);
#endregion

builder.Services.AddMudServices();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.
app.UseExceptionHandler("/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

app.Services.UseScheduler(scheduler =>
{
    scheduler
        .Schedule<ZotFlightServiceScheduler>()
        .Cron("* */5 * * *")
        .PreventOverlapping("ZotFlightServiceScheduler");
    scheduler
        .Schedule<FlyZenServiceScheduler>()
        .Cron("* */5 * * *")
        .PreventOverlapping("FlyZenServiceScheduler");
});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapControllers();
app.MapRpcWebSocketServer();
app.MapFallbackToPage("/_Host");

var dbContextFactory = app.Services.GetRequiredService<IDbContextFactory<AppDbContext>>();
using var dbContext = dbContextFactory.CreateDbContext();
//await dbContext.Database.MigrateAsync();
// await dbContext.Database.EnsureDeletedAsync();
dbContext.Database.EnsureCreated();
app.Run();

public partial class Program { }