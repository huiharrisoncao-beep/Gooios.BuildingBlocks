using AspectCore.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Gooios.BuildingBlocks.Interceptors;

public class AppServiceInterceptor : AbstractInterceptorAttribute
{
    public override async Task Invoke(AspectContext context, AspectDelegate next)
    {
        var logger = context.ServiceProvider.GetService<ILogger<AppServiceInterceptor>>();

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
            if (context.ReturnValue != null && context.ReturnValue.GetType() != typeof(MemoryStream) && context.ReturnValue.GetType() != typeof(Stream) && context.ReturnValue.GetType() != typeof(FileStream))
            {
                var res = JsonSerializer.Serialize(context.ReturnValue);
                logger?.LogInformation($"Result value: {res}");
            }
            else
            {
                logger?.LogInformation($"Result value is MemoryStream or null.");
            }
        }
    }
}