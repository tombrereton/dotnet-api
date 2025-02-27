namespace Teeitup.Web.Api.Common;

public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected Entity(Guid id)
    {
        Id = id;
    }
    protected Entity()
    {
    }

    public Guid Id { get; set; }
    public List<IDomainEvent> DomainEvents => _domainEvents.ToList();

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent) 
    {
        _domainEvents.Add(domainEvent);
    }

}
