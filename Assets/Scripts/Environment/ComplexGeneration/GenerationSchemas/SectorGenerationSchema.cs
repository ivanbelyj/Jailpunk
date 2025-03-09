using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "New Sector Generation Schema",
    menuName = "Complex Generation/Sector Generation Schema",
    order = 52)]
public class SectorGenerationSchema : ScriptableObject
{
    [System.Serializable]
    public class SectorZoneVariant {
        public SectorZoneGenerationSchema sectorZoneGenerationSchema;

        [Min(0f)]
        [Tooltip("Defines frequency of selection relatively to other zones")]
        public float weight = 1f;
    }

    public string sectorGenerationSchemaId;
    public SectorPlanningType planningType;

    public List<SectorZoneVariant> zoneVariants;
}

public enum SectorPlanningType {
    RoomsAndCorridors,
    Glade
}
