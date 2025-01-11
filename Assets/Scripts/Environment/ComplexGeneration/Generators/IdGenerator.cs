using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdGenerator
{
    private int lastId = 0;
    public int NewSectorId() => ++lastId;
    public int NewAreaId() => ++lastId;
}
