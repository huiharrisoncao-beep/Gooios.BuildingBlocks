using Gooios.BuildingBlocks.Domain.Seedwork;
using Gooios.BuildingBlocks.Seedwork;

namespace Gooios.BuildingBlocks.Application.Contracts;

public interface IApplicationServiceContract : IDisposable { }

public class ApplicationServiceContract : DisposableObject, IApplicationServiceContract
{
    protected readonly IUnitOfWork _dbUnitOfWork;

    public ApplicationServiceContract(IUnitOfWork dbUnitOfWork)
    {
        _dbUnitOfWork = dbUnitOfWork;
    }
}