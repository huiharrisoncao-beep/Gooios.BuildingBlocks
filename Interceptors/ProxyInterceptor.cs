using AspectCore.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Gooios.BuildingBlocks.Interceptors;

public class ProxyInterceptor : AbstractInterceptorAttribute
{
    public override async Task Invoke(AspectContext context, AspectDelegate next)
    {
        var logger = context.ServiceProvider.GetService<ILogger<ProxyInterceptor>>();
        var parameters = JsonSerializer.Serialize(context.Parameters);

        logger?.LogInformation($"parameters: {parameters}");

        await next(context);

        if (context.IsAsync())
        {
            if (context.ServiceMethod.ReturnType.FullName == "System.Threading.Tasks.Task")
            {
                logger?.LogInformation($"Result value: void");
            }
            else
            {
                var result = await context.UnwrapAsyncReturnValue();
                var res = JsonSerializer.Serialize(result);
                logger?.LogInformation($"Result value: {res}");
            }
        }
        else
        {
            if (context.ReturnValue != null)
            {
                var res = JsonSerializer.Serialize(context.ReturnValue);
                logger?.LogInformation($"Result value: {res}");
            }
        }
    }
}