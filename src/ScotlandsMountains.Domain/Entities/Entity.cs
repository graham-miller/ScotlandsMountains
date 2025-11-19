namespace ScotlandsMountains.Domain.Entities;

public abstract class Entity : IEquatable<Entity>
{
    public int Id { get; protected set; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Entity);
    }

    public virtual bool Equals(Entity? other)
    {
        if (other is null) return false;

        if (ReferenceEquals(this, other)) return true;

        if (IsTransient() || other.IsTransient())
        {
            return false;
        }

        if (other.GetType() != GetType()) return false;

        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return IsTransient() ? base.GetHashCode() : HashCode.Combine(Id, GetType());
    }
    
    public static bool operator ==(Entity left, Entity right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !Equals(left, right);
    }

    private bool IsTransient()
    {
        return Id <= 0;
    }
}