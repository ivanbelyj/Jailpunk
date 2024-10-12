using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationParameters
{
    public string State { get; set; }
    public int Angle { get; set; }
    public bool Flip { get; set; }

    /// <summary>
    /// Index of the last frame of 'walk' animation sequence before 
    /// it changed to 'idle'. For states, other than 'idle', null.
    /// </summary>
    public int? LastWalkFrame { get; set; }

    public override bool Equals(object obj)
    {
        if (obj is AnimationParameters other)
        {
            return State == other.State
                && Angle == other.Angle
                && Flip == other.Flip;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(State, Angle);
    }

    public override string ToString()
    {
        return $"{{State={State}, Angle={Angle},"
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
