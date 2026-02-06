using Autofac;
using Serilog;

namespace Gooios.BuildingBlocks.Modules.Log;

public class LoggingModule : Module
{
    private readonly ILogger _logger;

    public LoggingModule(ILogger logger)
    {
        _logger = logger;
    }

    protected override void Load(ContainerBuilder builder)
    {
        if (_logger != null)
            builder.RegisterInstance(_logger)
            .As<ILogger>()
            .SingleInstance();
    }
}