using Gooios.BuildingBlocks.Domain.Repository;
using Gooios.BuildingBlocks.Domain.Seedwork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Gooios.BuildingBlocks.Infrastructure.Repository;

public class Repository<E, K, TDbContext> : IRepository<E, K> 
    where E : Entity<K>
    where TDbContext : DbContext
{
    TDbContext? _ServiceDbContext = null;

    protected TDbContext ServiceDbContext => _ServiceDbContext!;

    private readonly DbSet<E> _dbset;

    public Repository(IDbContextProvider<TDbContext> dbContextProvider)
    {
        var dbContext = dbContextProvider.GetDbContext();
        _ServiceDbContext = dbContext;
        _dbset = _ServiceDbContext.Set<E>();
    }

    public virtual async Task AddAsync(E item, CancellationToken cancellationToken = default)
    {
        await _dbset.AddAsync(item);
    }

    public virtual async Task AddRangeAsync(IEnumerable<E> items, CancellationToken cancellationToken = default)
    {
        await _dbset.AddRangeAsync(items);
    }

    public virtual async Task RemoveAsync(E item, CancellationToken cancellationToken = default)
    {
        _dbset.Remove(item);
        await Task.CompletedTask;
    }

    public virtual async Task RemoveRangeAsync(IEnumerable<E> items, CancellationToken cancellationToken = default)
    {
        _dbset.RemoveRange(items);
        await Task.CompletedTask;
    }

    public virtual async Task UpdateAsync(E item, CancellationToken cancellationToken = default)
    {
        _dbset.Attach(item);
        _ServiceDbContext!.Entry(item).State = EntityState.Modified;

        await Task.CompletedTask;
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<E> items, CancellationToken cancellationToken = default)
    {
        _dbset.AttachRange(items);
        foreach (var item in items)
            _ServiceDbContext!.Entry(item).State = EntityState.Modified;

        await Task.CompletedTask;
    }

    public async Task<E?> GetAsync(K id, CancellationToken cancellationToken = default)
    {
        return await _dbset.FindAsync(id);
    }

    public async Task<E?> GetAsync(Expression<Func<E, bool>> where, CancellationToken cancellationToken = default)
    {
        return await _dbset.FirstOrDefaultAsync(where);
    }

    public async Task<IEnumerable<E>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbset.ToListAsync();
    }

    public async Task<IEnumerable<E>> FindAsync(Expression<Func<E, bool>> filter, CancellationToken cancellationToken = default)
    {
        return await _dbset.Where(filter).ToListAsync();
    }

    public async Task<IEnumerable<E>> GetPagedAsync<KProperty>(int pageIndex, int pageCount, Expression<Func<E, KProperty>> orderByExpression, bool ascending, CancellationToken cancellationToken = default)
    {
        return ascending ? await _dbset.OrderBy(orderByExpression).Skip(pageCount * pageIndex).Take(pageCount).ToListAsync()
                         : await _dbset.OrderByDescending(orderByExpression).Skip(pageCount * pageIndex).Take(pageCount).ToListAsync();
    }

    public async Task<IEnumerable<E>> GetPagedAsync<KProperty>(int pageIndex, int pageCount, Expression<Func<E, bool>> filter, Expression<Func<E, KProperty>> orderByExpression, bool ascending, CancellationToken cancellationToken = default)
    {
        IQueryable<E> query = filter != null ? _dbset.Where(filter) : _dbset;
        query = ascending ? query.OrderBy(orderByExpression) : query.OrderByDescending(orderByExpression);

        return await query.Skip(pageCount * pageIndex).Take(pageCount).ToListAsync();
    }
}
