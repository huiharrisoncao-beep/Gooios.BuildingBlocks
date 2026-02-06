using Gooios.BuildingBlocks.Infrastructure.Repository;
using Gooios.BuildingBlocks.Modules.BusinessLogs.Domain;
using Microsoft.EntityFrameworkCore;

namespace Gooios.BuildingBlocks.Modules.BusinessLogs.Repository;

public class BusinessLogRepository<TDbContext> : Repository<BusinessLog, string, TDbContext>, IBusinessLogRepository
    where TDbContext : DbContext
{
    public BusinessLogRepository(IDbContextProvider<TDbContext> provider) : base(provider) { }
}

