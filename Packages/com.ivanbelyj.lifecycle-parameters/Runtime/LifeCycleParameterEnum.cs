using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Allows ids for entity lifecycle parameters (their set is statically limited).
/// You can define your custom enum for your parameters
///</summary>
public enum LifecycleParameterEnum : byte
{
    Health,
    Endurance,
    Satiety,
    Bleeding,
    Radiation
}
