using AspectCore.DynamicProxy;
using Gooios.BuildingBlocks.Application.Transaction;
using Gooios.BuildingBlocks.Domain.Seedwork;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Gooios.BuildingBlocks.Interceptors;

public class TransactionInterceptor : AbstractInterceptorAttribute
{

    public override async Task Invoke(AspectContext context, AspectDelegate next)
    {
        var logger = context.ServiceProvider.GetService<ILogger<TransactionInterceptor>>();

        try
        {
            var enableTransaction = false;
            IDbUnitOfWork? dbUnitOfWork = null;

            if (context.ImplementationMethod.IsDefined(typeof(EnableTransactionAttribute), false))
            {
                enableTransaction = true;

                var custAttr = (EnableTransactionAttribute?)GetCustomAttribute(context.ImplementationMethod, typeof(EnableTransactionAttribute));

                if (custAttr == null) 
                    throw new CanNotGetAttributeException(500, "Occur a exception when get the attribute from the target method.(Can not get the attribute named \"EnableTransactionAttribute\")");

                var flag = BindingFlags.Instance | BindingFlags.NonPublic;
                dbUnitOfWork = (IDbUnitOfWork?)(context.Implementation.GetType().GetField("_dbUnitOfWork", flag)?.GetValue(context.Implementation));
                
                if (dbUnitOfWork == null)
                    throw new Exception("Cannot find the 'dbUnitOfWork!'");
            }

            using var scope = new TransactionContext(dbUnitOfWork!, enableTransaction);
            await next(context);
            
        }
        catch (System.Exception ex)
        {
            logger?.LogError(ex, ex.Message);
            throw;
        }
    }
}