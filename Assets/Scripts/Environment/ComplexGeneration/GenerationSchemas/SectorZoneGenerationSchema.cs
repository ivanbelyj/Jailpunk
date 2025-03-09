using System.Collections.Generic;
using UnityEngine;

public enum ZoneFillingType {
    MazeWithRooms,
    Empty,
    Test
}

[System.Serializable]
public class ZoneFillingLayerSchema {
    public ZoneFillingType fillingType;
    public TraverseRectFilter traverseRectFilter;
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
