namespace Gooios.BuildingBlocks.Application.Contracts;

public record Pagination(int? PageIndex = null, int? PageSize = null, long? TotalCount = null);
