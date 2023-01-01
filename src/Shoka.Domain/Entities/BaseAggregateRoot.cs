namespace Shoka.Domain.Entities;

public abstract class BaseAggregateRoot:Entity,IAggregateRoot
{
    
}

public abstract class BaseAggregateRoot<TKey> : Entity<TKey>,
IAggregateRoot<TKey>
{    
    protected BaseAggregateRoot(){}
    
    protected BaseAggregateRoot(TKey id) : base(id)
    {
    }
}