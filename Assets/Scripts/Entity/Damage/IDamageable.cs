using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Entity that can get damage
/// </summary>
public interface IDamageable
{
    void Damage(Damage damage);
}
