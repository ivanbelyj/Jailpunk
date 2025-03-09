using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneAreaAllocatorFilter : IAreaAllocatorAreaFilter
{
    private readonly IEnumerable<SchemeArea> sectorAreas;

    public ZoneAreaAllocatorFilter(IEnumerable<SchemeArea> sectorAreas)
    {
        this.sectorAreas = sectorAreas;
    }

    public bool ShouldAllocate(int areaId)
    {
        return sectorAreas.First(x => x.Id == areaId).Type == SchemeAreaType.CoreArea;
    }
}
