using Autofac;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace Gooios.BuildingBlocks.Modules.Mediations;

public class MediatorModule : Autofac.Module
{
    private Assembly _assemblyNeedToRegister;

    public MediatorModule(Assembly assemblyNeedToRegister)
    {
        _assemblyNeedToRegister = assemblyNeedToRegister;
    }
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

        var mediatorOpenTypes = new[]
            {
            typeof(IRequestHandler<,>),
            typeof(INotificationHandler<>),
            typeof(IValidator<>)
        };

        foreach (var mediatorOpenType in mediatorOpenTypes)
        {
            builder.RegisterAssemblyTypes(_assemblyNeedToRegister)
                   .AsClosedTypesOf(mediatorOpenType)
                   .AsImplementedInterfaces();
        }
    }
}