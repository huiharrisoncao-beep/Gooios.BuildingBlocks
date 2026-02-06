using Autofac;
using Gooios.BuildingBlocks.Modules.BusinessLogs.Domain;
using Gooios.BuildingBlocks.Modules.BusinessLogs.Repository;
using Microsoft.EntityFrameworkCore;

namespace Gooios.BuildingBlocks.Modules.BusinessLogs;

public class BusinessLogsModule<TDbContext> : Module
    where TDbContext : DbContext
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<BusinessLogRepository<TDbContext>>()
               .As<IBusinessLogRepository>()
               .InstancePerLifetimeScope();
    }
}
