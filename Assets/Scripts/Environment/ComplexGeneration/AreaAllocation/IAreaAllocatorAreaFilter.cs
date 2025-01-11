using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAreaAllocatorAreaFilter
{
    bool ShouldAllocate(int areaId);
}
