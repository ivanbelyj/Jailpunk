using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SectorPlanningInfo {
    public string SectorGenerationSchemaId { get; set; }
}

public class GeneratedSectorInfo
{
    public int Id { get; set; }
    public RectArea RectArea { get; set; }
    public List<SchemeArea> SchemeAreas { get; set; }
    
    public List<GeneratedZone> Zones { get; set; } = new();

    public SectorPlanningInfo SectorPlanningInfo { get; set; }
}
