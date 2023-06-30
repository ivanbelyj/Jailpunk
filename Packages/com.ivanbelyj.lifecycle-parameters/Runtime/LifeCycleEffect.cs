using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Effect applied to the life cycle parameter
///</summary>
[System.Serializable]
public struct LifecycleEffect : IEquatable<LifecycleEffect>
{
    public bool isInfinite;

    [Tooltip("Value, added / subtracted from the parameter value per second")]
    /// <summary>
    /// Value, added / subtracted from the parameter value per second
    /// </summary>
    public float speed;

    
    [Tooltip("Effect duration in seconds")]
    /// <summary>
    /// Effect duration in seconds
    /// </summary>
    public float duration;
    
    /// <summary>
    /// Every effect is applied by parameter id
    /// </summary>
    public uint targetParameterId;

    public double StartTime { get; set; }

    public override bool Equals(object obj)
    {
        return obj is LifecycleEffect effect && Equals(effect);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(isInfinite, speed, duration, targetParameterId, StartTime);
    }

    public bool Equals(LifecycleEffect other) {
        return isInfinite == other.isInfinite
            && duration == other.duration
            && speed == other.speed
            && targetParameterId == other.targetParameterId
            && StartTime == other.StartTime;
    }

    public override string ToString()
    {
        return "{" + $" isInfinite: {isInfinite}; speed: {speed}; duration: {duration};"
            + $" targetParameter: {targetParameterId}; StartTime: {StartTime}" + "}";
    }
}
