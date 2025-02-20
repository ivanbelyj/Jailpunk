using System;

/// <summary>
/// Defines animation features actual for all the elements of the appearance
/// </summary>
public struct AppearanceAnimationData : IEquatable<AppearanceAnimationData>
{
    public string State { get; set; }
    public int Angle { get; set; }
    public int Frame { get; set; }

    // Todo: Define it for appearance elements individually
    public bool Flip { get; set; }

    /// <summary>
    /// Index of the last frame of 'walk' animation sequence before 
    /// it changed to 'idle'. For states, other than 'idle', null.
    /// </summary>
    public int? LastWalkFrame { get; set; }

    public override bool Equals(object obj)
    {
        if (obj is AppearanceAnimationData other)
        {
            return Equals(other);
        }
        return false;
    }

    public bool Equals(AppearanceAnimationData other)
    {
        return State == other.State
            && Angle == other.Angle
            && Flip == other.Flip;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(State, Angle);
    }

    public override string ToString()
    {
        return $"{{State={State}, Angle={Angle}, "
            + $"Flip={Flip}}}";
    }

    public static bool operator ==(AppearanceAnimationData left, AppearanceAnimationData right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(AppearanceAnimationData left, AppearanceAnimationData right)
    {
        return !(left == right);
    }
}
