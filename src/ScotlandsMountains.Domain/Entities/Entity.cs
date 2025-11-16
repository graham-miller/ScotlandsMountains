namespace ScotlandsMountains.Domain.Entities;

public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull
{
    public TId Id { get; protected set; }

    protected Entity()
    {
        Id = default!;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Entity<TId>);
    }

    public virtual bool Equals(Entity<TId>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        if (IsTransient(Id))
        {
            return ReferenceEquals(this, other);
        }

        if (other.GetType() != GetType()) return false;

        return Id.Equals(other.Id);
    }

    private bool IsTransient(TId id)
    {
        return EqualityComparer<TId>.Default.Equals(id, default!);
    }

    public override int GetHashCode()
    {
        return IsTransient(Id) ? base.GetHashCode() : HashCode.Combine(Id, GetType());
    }
    
    public static bool operator ==(Entity<TId> left, Entity<TId> right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity<TId> left, Entity<TId> right)
    {
        return !Equals(left, right);
    }
}