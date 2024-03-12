using System.Data;
using ActualLab;
using ActualLab.Fusion.EntityFramework;
using ActualLab.Fusion.EntityFramework.Operations;
using ActualLab.Multitenancy;
using Aggregator.Data;
using Microsoft.EntityFrameworkCore;

namespace Aggregator.Infrastructure.ServiceCollection;

public static class AddDb
{
    public static IServiceCollection AddDataBase<TContext>(this IServiceCollection services, IWebHostEnvironment env,
        ConfigurationManager cfg) where TContext : DbContext
    {
        services.AddTransient(_ => new DbOperationScope<TContext>.Options
        {
            DefaultIsolationLevel = IsolationLevel.RepeatableRead
        });

        services.AddDbContextServices<TContext>(ctx =>
        {
            ctx.AddOperations(operations =>
            {
                operations.ConfigureOperationLogReader(_ => new DbOperationLogReader<TContext>.Options
                {
                    UnconditionalCheckPeriod = TimeSpan.FromSeconds(5)
                });

                operations.AddFileBasedOperationLogChangeTracking();
            });

            ctx.Services.AddDbContextFactory<TContext>((c, db) =>
            {
                var fakeTenant = new Tenant(default, "single", "single");
                var dbPath = "/App_{0:StorageId}.db".Interpolate(fakeTenant);
                db.UseSqlite($"Data Source={Directory.GetCurrentDirectory() + dbPath}",
                    x => x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            });
        });

        return services;
    }
}