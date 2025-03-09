using System.Collections.Generic;
using UnityEngine;

public enum ZoneFillingType {
    Empty = 1,
    Solid = 2,
    MazeWithRooms = 3,
    Test = 4
}

[CreateAssetMenu(
    fileName = "New Sector Zone Generation Schema",
    menuName = "Complex Generation/Sector Zone Generation Schema",
    order = 52)]
public class SectorZoneGenerationSchema : ScriptableObject
{
    public string sectorZoneGenerationSchemaId;
    public List<ZoneFillingLayerSchema> zoneFillingSchemas;
}
