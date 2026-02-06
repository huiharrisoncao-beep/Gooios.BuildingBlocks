using Gooios.BuildingBlocks.Application.Transaction;
using Gooios.BuildingBlocks.Domain.Seedwork;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Gooios.BuildingBlocks.Interceptors;

public class MediatRPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    readonly ILogger<MediatRPipelineBehavior<TRequest, TResponse>> _logger;
    readonly IServiceProvider _serviceProvider;

    public MediatRPipelineBehavior(IServiceProvider serviceProvider, ILogger<MediatRPipelineBehavior<TRequest, TResponse>> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            var enableTransaction = false;
            IDbUnitOfWork? dbUnitOfWork = null;

            if (request.GetType().IsDefined(typeof(EnableTransactionAttribute), false))
            {
                enableTransaction = true;
                BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
                var handler = request.GetType().Assembly.DefinedTypes.Where(o => o.FullName == $"{request.GetType().FullName}Handler").FirstOrDefault()!.BaseType;
                if (handler == null) throw new Exception("Can not get the specified handler.");

                var implementType = request.GetType().Assembly.GetTypes().Where(o => o.IsAssignableTo(handler)).FirstOrDefault();
                if (implementType == null) throw new Exception("Can not get the implement type.");

                var obj = _serviceProvider.GetService(implementType);
                dbUnitOfWork = (IDbUnitOfWork?)(handler.GetField("_dbUnitOfWork", flag)?.GetValue(obj));

                if (dbUnitOfWork == null)
                    throw new Exception("Cannot find the 'dbUnitOfWork!'");

            }

            using var scope = new TransactionContext(dbUnitOfWork!, enableTransaction);
            var response = await next();
            return response;

        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, ex.Message);
            throw;
        }
    }
}