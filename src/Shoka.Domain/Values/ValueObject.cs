namespace Shoka.Domain.Values;

public abstract class ValueObject : IValueObject
{
    protected abstract IEnumerable<object> GetAttributesToIncludeInEqualityCheck();

    public virtual bool Equals(ValueObject obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return GetAttributesToIncludeInEqualityCheck().SequenceEqual(obj.GetAttributesToIncludeInEqualityCheck());
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj as ValueObject);
    }

    public static bool operator ==(ValueObject left, ValueObject right)
    { return Equals(left, right); }


    public static bool operator !=(ValueObject left, ValueObject right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        var hash = 17;
        foreach (var obj in this.GetAttributesToIncludeInEqualityCheck())
            hash = hash * 31 + (obj == null ? 0 : obj.GetHashCode());
        return hash;
    }

}
