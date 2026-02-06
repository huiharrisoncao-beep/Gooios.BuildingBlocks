namespace Gooios.BuildingBlocks.Domain.Seedwork;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}