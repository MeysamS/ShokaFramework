
using Shoka.Core.Collection;

namespace Shoka.Domain.Entities;

public abstract class Entity : IEntity
{
    public abstract object[] GetKeys();


    public override string ToString()
    {
        return $"[Entity: {GetType().Name}] Keys= {GetKeys().JoinAsString(", ")}";
    }

}

[Serializable]
public abstract class Entity<TKey> : Entity, IEntity<TKey>, IEquatable<Entity<TKey>>
{
    public virtual TKey Id { get; protected set; }

    protected Entity(TKey id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        var target = obj as Entity<TKey>;
        if (target == null)
            return false;
        if (ReferenceEquals(this, obj) == false)
            return false;
        if (obj?.GetType() != GetType())
            return false;
        return Equals((Entity<TKey>)obj);

    }

    public bool Equals(Entity<TKey>? other)
    {
        if (other == null) return false;
        if (other.GetType() != GetType()) return false;
        return Id?.Equals(other.Id) ?? false;
    }

    public override int GetHashCode()
    {
        return EqualityComparer<TKey>.Default.GetHashCode(Id!);
    }

    public static bool operator ==(Entity<TKey>? left, Entity<TKey>? right)
    {
        if (Equals(left, null))
            return Equals(right, null) ? true : false;
        else
            return left.Equals(right);
    }

    public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
    {
        return !(left == right);
    }
}

