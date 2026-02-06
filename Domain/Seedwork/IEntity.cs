using Gooios.BuildingBlocks.Domain.Event;
using Gooios.BuildingBlocks.Domain.Exceptions;
using System.Text.Json.Serialization;

namespace Gooios.BuildingBlocks.Domain.Seedwork;

public interface IEntity
{
    string CreatedBy { get; }

    DateTime CreatedOn { get; }

    string UpdatedBy { get; }

    DateTime UpdatedOn { get; }

    [JsonIgnore]
    IReadOnlyCollection<IDomainEvent>? DomainEvents { get; }

    void ClearDomainEvents();
}

public abstract class Entity<T> : IEntity
{
    T _id = default!;

    public virtual T Id
    {
        get { return _id; }
        protected set { _id = value; }
    }

    public virtual string CreatedBy { get; set; } = null!;

    public virtual DateTime CreatedOn { get; set; }

    public virtual string UpdatedBy { get; set; } = null!;

    public virtual DateTime UpdatedOn { get; set; }

    protected async Task CheckRule(IBusinessRule rule)
    {
        if (await rule.IsBroken())
            throw new BusinessRuleValidationException(rule);
    }

    protected async Task<(bool,string?)> CheckRuleReturnMessage(IBusinessRule rule)
    {
        if (await rule.IsBroken())
            return (false, new BusinessRuleValidationException(rule).ToString());

        return (true, null);
    }

    #region Domain events

    private List<IDomainEvent>? _domainEvents = new List<IDomainEvent>();

    [JsonIgnore]
    public IReadOnlyCollection<IDomainEvent>? DomainEvents => _domainEvents?.AsReadOnly();

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }

    protected void PublishDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents ??= new List<IDomainEvent>();

        this._domainEvents.Add(domainEvent);
    }

    #endregion
}