using Gooios.BuildingBlocks.Domain.Seedwork;

namespace Gooios.BuildingBlocks.Infrastructure.Repository;

public interface IEFUnitOfWork<TDbContext> : IDbUnitOfWork
{
    TDbContext DatabaseContext { get; }
}