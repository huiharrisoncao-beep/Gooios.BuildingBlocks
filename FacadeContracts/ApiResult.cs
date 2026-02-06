namespace Gooios.BuildingBlocks.FacadeContracts;

public record ApiResult<T>(int Code, T? Result, string? Message);
