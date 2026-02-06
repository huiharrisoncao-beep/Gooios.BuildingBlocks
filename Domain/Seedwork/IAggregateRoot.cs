namespace Gooios.BuildingBlocks.Domain.Seedwork;

public interface IAggregateRoot : IEntity { }

public class AggregateRoot<T> : Entity<T>, IAggregateRoot { }
