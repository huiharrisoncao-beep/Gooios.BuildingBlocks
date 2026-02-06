namespace Gooios.BuildingBlocks.Modules.Consistency.Outbox;

public class ModuleConfiguration
{
    public string IntegrationEventAssemblyName { get; private set; } = default!;

    public ModuleConfiguration(string integrationEventAssemblyName)
    {
        IntegrationEventAssemblyName = integrationEventAssemblyName;
    }
}
