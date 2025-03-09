using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "New Complex Generation Schema",
    menuName = "Complex Generation/Complex Generation Schema",
    order = 52)]
public class ComplexGenerationSchema : ScriptableObject
{
    [System.Serializable]
    public class ComplexSectorVariant {
        public SectorGenerationSchema sectorGenerationSchema;

        [Min(0f)]
        [Tooltip("Defines frequency of selection relatively to other sectors")]
        public float weight = 1f;
    }

    public string complexGenerationSchemaId;
    public List<ComplexSectorVariant> sectorVariants;
}
