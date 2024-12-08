using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorIdGenerator
{
    private int lastSectorId = 0;
    public int NewSectorId() => ++lastSectorId;
}
