using Gooios.BuildingBlocks.Modules.BusinessLogs.Domain;

namespace Gooios.BuildingBlocks.Modules.BusinessLogs.Application;

public record BusinessLogDto(string Id,
                             string? OperateName,
                             string? SchemaName,
                             string? SchemaKeyValue,
                             string? OldValue,
                             string? NewValue,
                             OperationLogStatus? Status,
                             string CreatedBy,
                             DateTime CreatedOn,
                             string UpdatedBy,
                             DateTime UpdatedOn);
