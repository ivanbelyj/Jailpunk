using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdGenerator
{
    private int lastSectorId = 0;
    private int lastAreaId = 0;
    public int NewSectorId() => ++lastSectorId;
    public int NewAreaId() => ++lastAreaId;
}
