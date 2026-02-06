namespace Gooios.BuildingBlocks.Application.Contracts;

public record BaseDto(long Id, string CreatedBy, DateTime CreatedOn, string? UpdatedBy, DateTime? UpdatedOn);
