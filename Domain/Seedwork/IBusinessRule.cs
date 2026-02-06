namespace Gooios.BuildingBlocks.Domain.Seedwork;

public interface IBusinessRule
{
    Task<bool> IsBroken();

    string Message { get; }
}