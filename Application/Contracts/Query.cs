using Gooios.BuildingBlocks.Seedwork;

namespace Gooios.BuildingBlocks.Application.Contracts;

public record Query<T> : IQuery<T>
{
    public Guid Id { get; }

    public Query()
    {
        Id = Guid.NewGuid();
    }
    public Query(Guid id)
    {
        Id = id;
    }
}

public abstract class QueryHandler<TCommand, TResponse> : DisposableObject, IQueryHandler<TCommand, TResponse>
   where TCommand : IQuery<TResponse>
{
    public QueryHandler()
    {
    }

    public abstract Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken);
}