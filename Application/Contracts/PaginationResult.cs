namespace Gooios.BuildingBlocks.Application.Contracts;

public record PaginationResult<T>(IEnumerable<T> Result, Pagination? Pagnation);
