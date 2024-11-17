using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Damage
{
    /// <summary>
    /// Force (usually in hit points)
    /// </summary>
    public float force;

    public DamageType damageType;

    /// <summary>
    /// Knockout force. Used for damage of type Punch
    /// </summary>
    // publicf float punchForce;

    public override string ToString()
    {
        return (force, damageType).ToString();
    }
}
