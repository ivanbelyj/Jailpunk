using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum RenewalConfig
{
    RefreshObjects = 1 << 0,
    PassOut = 1 << 1
}
