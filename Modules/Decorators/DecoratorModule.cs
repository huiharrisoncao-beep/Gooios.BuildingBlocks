using Gooios.BuildingBlocks.Application.Contracts;
using Gooios.BuildingBlocks.Infrastructure.Repository;
using Autofac;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gooios.BuildingBlocks.Modules.Decorators;

internal class DecoratorModule<TDbContext> : Module
    where TDbContext : DbContext
{
    private IDbConfiguration _dbConfiguration;

    public DecoratorModule(IDbConfiguration dbConfiguration)
    {
        _dbConfiguration = dbConfiguration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(_dbConfiguration).SingleInstance();

        //Just used to initialize DbContext with correct DbContextOptions
        builder.Register(c =>
                {
                    var dbContextOptionsBuilder = new DbContextOptionsBuilder<TDbContext>();

                    if (_dbConfiguration.DatabaseType == DatabaseType.PostgreSql)
                    {
                        dbContextOptionsBuilder.UseNpgsql(_dbConfiguration.ConnectionString, x => x.UseNetTopologySuite());
                    }
                    else
                    {
                        dbContextOptionsBuilder.UseMySql(_dbConfiguration.ConnectionString, new MySqlServerVersion("8.0"));
                    }

                    Type genericDbContextType = typeof(TDbContext);
                    object instance = Activator.CreateInstance(genericDbContextType, dbContextOptionsBuilder.Options)!;
                    return (TDbContext)instance;
                })
               .InstancePerLifetimeScope();

        builder.RegisterType<DbContextProvider<TDbContext>>()
               .As<IDbContextProvider<TDbContext>>()
               .InstancePerLifetimeScope();

        builder.RegisterType<EFUnitOfWork<TDbContext>>()
               .As<IEFUnitOfWork<TDbContext>>()
               .InstancePerLifetimeScope();

        builder.RegisterGenericDecorator(
            typeof(UnitOfWorkCommandHandlerWithResultDecorator<,,>),
            typeof(ICommandHandler<,>));

        builder.RegisterGenericDecorator(
            typeof(LoggingCommandHandlerWithResultDecorator<,>),
            typeof(ICommandHandler<,>));

        builder.RegisterGenericDecorator(
            typeof(ValidationCommandHandlerWithResultDecorator<,>),
            typeof(IRequestHandler<,>));
    }
}
