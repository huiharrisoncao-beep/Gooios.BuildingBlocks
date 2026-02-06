using Gooios.BuildingBlocks.Domain.Seedwork;
using System.Linq.Expressions;

namespace Gooios.BuildingBlocks.Domain.Repository;

public interface IRepository<TEntity, TKey>
   where TEntity : Entity<TKey>
{
    Task AddAsync(TEntity item, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default);

    Task RemoveAsync(TEntity item, CancellationToken cancellationToken = default);

    Task RemoveRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default);

    /// <summary>
    /// Set item as modified
    /// </summary>
    /// <param name="item">Item to modify</param>
    Task UpdateAsync(TEntity item, CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default);

    Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken = default);

    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all elements of type TEntity in repository
    /// </summary>
    /// <returns>List of selected elements</returns>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all elements of type TEntity in repository
    /// </summary>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageCount">Number of elements in each page</param>
    /// <param name="orderByExpression">Order by expression for this query</param>
    /// <param name="ascending">Specify if order is ascending</param>
    /// <returns>List of selected elements</returns>
    Task<IEnumerable<TEntity>> GetPagedAsync<KProperty>(int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending, CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> GetPagedAsync<KProperty>(int pageIndex, int pageCount, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get  elements of type TEntity in repository
    /// </summary>
    /// <param name="filter">Filter that each element do match</param>
    /// <returns>List of selected elements</returns>
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);
}
