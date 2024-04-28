using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Entity that can get damage
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Damage value in [0, 1]
    /// </summary>
    void Damage(float damageVal);
}
