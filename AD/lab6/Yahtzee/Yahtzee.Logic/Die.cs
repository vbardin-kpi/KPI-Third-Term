namespace Yahtzee.Logic;

public class Die : IComparable<Die>
{
    public int Value { get; set; }
    public bool IsHold { get; set; }

    public int CompareTo(Die? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Value.CompareTo(other.Value);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Value == ((Die) obj).Value;
    }

    [Obsolete("This method method must not be used! It uses not-readonly parameter", true)]
    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return Value;
    }
}