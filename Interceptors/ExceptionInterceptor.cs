using AspectCore.DynamicProxy;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Gooios.BuildingBlocks.Interceptors;

public class ExceptionInterceptor : AbstractInterceptorAttribute
{
    public override async Task Invoke(AspectContext context, AspectDelegate next)
    {
        var logger = context.ServiceProvider.GetService<ILogger<ExceptionInterceptor>>();

        try
        {
            await next(context);
        }
        catch (System.Exception ex)
        {
            logger?.LogError(ex, ex.Message);
            throw;
        }
    }
}