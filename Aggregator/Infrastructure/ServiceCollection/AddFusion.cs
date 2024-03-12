using System.Data;
using ActualLab;
using ActualLab.Fusion;
using ActualLab.Fusion.Blazor;
using ActualLab.Fusion.EntityFramework;
using ActualLab.Fusion.EntityFramework.Operations;
using ActualLab.Fusion.Server;
using ActualLab.Multitenancy;
using ActualLab.Rpc;
using Aggregator.Data;
using Microsoft.EntityFrameworkCore;

namespace Aggregator.Infrastructure.ServiceCollection;

public static partial class ServiceCollection
{
    public static IServiceCollection AddFusionService(this IServiceCollection services) 
    {
        IComputedState.DefaultOptions.FlowExecutionContext = true;
        var fusion = services.AddFusion(RpcServiceMode.Server, true);
        fusion.AddWebServer();
        fusion.AddOperationReprocessor();
        fusion.AddBlazor();

        return services;
    }
}