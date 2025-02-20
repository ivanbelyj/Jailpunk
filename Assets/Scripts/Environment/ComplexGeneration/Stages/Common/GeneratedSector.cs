using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedSectorInfo
{
    public int Id { get; set; }
    public RectArea RectArea { get; set; }
    public List<SchemeArea> SchemeAreas { get; set; }

    /// <summary>
    /// Requested sector (not null if it was requested)
    /// </summary>
    // public SectorInfo RequestedSector { get; set; }
    
    // public List<AllocatableAreaGroup> ZoneGroups { get; set; } = new();
    public List<GeneratedZone> Zones { get; set; } = new();
}
