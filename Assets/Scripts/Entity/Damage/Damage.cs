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
}
