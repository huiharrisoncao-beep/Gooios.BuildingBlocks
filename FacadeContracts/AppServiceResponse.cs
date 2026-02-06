using Gooios.BuildingBlocks.Application.Contracts;

namespace Gooios.BuildingBlocks.FacadeContracts;

public record AppServiceResponse<T>
{
    public AppServiceResponse()
    {
        Code = ResponseCode.Successfully;
        CheckMessages = new List<string>();
        Message = string.Empty;
        Pagnation = null;
    }

    public AppServiceResponse(T? data) : this()
    {
        Data = data;
    }

    public ResponseCode Code { get; set; }

    public IEnumerable<string> CheckMessages { get; set; } = new List<string>();

    public string Message { get; set; } = null!;

    public T? Data { get; set; }

    public Pagination? Pagnation { get; set; } = null;
}

public record AppServiceResponse
{
    public AppServiceResponse()
    {
        Code = ResponseCode.Successfully;
        CheckMessages = new List<string>();
        Message = string.Empty;
        Pagnation = null;
        Data = null;
    }

    public AppServiceResponse(object data) : this()
    {
        Data = data;
    }

    public ResponseCode Code { get; set; }

    public IEnumerable<string> CheckMessages { get; set; } = new List<string>();

    public string Message { get; set; } = string.Empty;

    public object? Data { get; set; } = null;

    public Pagination? Pagnation { get; set; } = null;
}

public enum ResponseCode
{
    Failure = 0,
    Successfully = 1,
    ParameterInvalid = 2,
    NotFound = 4,
    ServerError = 5
}