using Gooios.BuildingBlocks.Domain.Seedwork;
using Gooios.BuildingBlocks.Seedwork;

namespace Gooios.BuildingBlocks.Application.Transaction;

/// <summary>
/// example:
/// public void AddVerification(VerificationDTO verificationDTO)
/// {
///     using (var coordinator = new TransactionCoordinator(_dbUnitOfWork, _eventBus))
///     {
///         var verifications = _verificationRepository.GetFiltered(o => o.IsSuspend == false && o.To == verificationDTO.To && o.BizCode == verificationDTO.BizCode).ToList();
///         _verificationService.SetVerificationsSuspend(verifications);
///
///         var verification = VerificationFactory.CreateVerification(verificationDTO.BizCode, verificationDTO.To);
///         verification.CreatedConfirm();
///         _verificationRepository.Add(verification);
///
///         coordinator.Commit();
///     }
/// }
/// </summary>

[Obsolete("Using the outbox instead")]
public interface ITransactionCoordinator : ITransactionUnitOfWork, IDisposable
{
}

[Obsolete("Using the outbox instead")]
public sealed class TransactionCoordinator : DisposableObject, ITransactionCoordinator
{
    private readonly List<IUnitOfWork> managedUnitOfWorks = new List<IUnitOfWork>();

    public TransactionCoordinator(params IUnitOfWork[] unitOfWorks)
    {
        if (unitOfWorks != null &&
            unitOfWorks.Length > 0)
        {
            foreach (var uow in unitOfWorks)
                managedUnitOfWorks.Add(uow);
        }
    }

    #region IUnitOfWork Members

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (managedUnitOfWorks.Count > 0)
            foreach (var uow in managedUnitOfWorks)
                await uow.CommitAsync();
    }

    #endregion
}