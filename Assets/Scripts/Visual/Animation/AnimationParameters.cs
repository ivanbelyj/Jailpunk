using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationParameters
{
    public string AnimationBase { get; set; }
    public int Angle { get; set; }
    public bool Flip { get; set; }

    public override bool Equals(object obj)
    {
        if (obj is AnimationParameters other)
        {
            return AnimationBase == other.AnimationBase
                && Angle == other.Angle
                && Flip == other.Flip;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(AnimationBase, Angle);
    }

    public override string ToString()
    {
        return $"{{AnimationBase={AnimationBase}, Angle={Angle},"
            + $" Flip={Flip}}}";
    }

    public static bool operator ==(AnimationParameters left, AnimationParameters right)
    {
        if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
        {
            return true;
        }
        if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
        {
            return false;
        }
        return left.Equals(right);
    }

    public static bool operator !=(AnimationParameters left, AnimationParameters right)
    {
        return !(left == right);
    }
}
