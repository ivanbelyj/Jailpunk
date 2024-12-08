using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SectorIndividualAccessibility {
    Isolated = 0,
    Low = 1,
    Medium = 2,
    High = 3,
}

public enum NecessityDegree {
    // /// <summary>
    // /// Will be implemented during the generation if it's possible,
    // /// depending on the defined probability
    // /// </summary>
    // Probable = 0,

    /// <summary>
    /// Will be implemented during the generation if it's possible
    /// </summary>
    Desirable = 1,

    /// <summary>
    /// There will be throwed an exception if the required won't be
    /// implemented during generation
    /// </summary>
    Required = 2
}

/// <summary>
/// Sector requirements defined on higher level by the plot necessity
/// </summary>
[System.Serializable]
public class RequestedSectorInfo
{
    [SerializeField] private int sectorGroupId;
    [SerializeField]
    private bool useIndividualAccessibility = true;
    [SerializeField]
    private SectorIndividualAccessibility individualAccessibility = SectorIndividualAccessibility.High;
    [SerializeField]
    private NecessityDegree necessity = NecessityDegree.Required;

    // [Tooltip("Used for sectors with necessity degree set to 'probable'")]
    // [Range(0f, 1f)]
    // [SerializeField] private float probability = 0.5f;

    public int SectorGroupId => sectorGroupId;
    public bool UseIndividualAccessibility => useIndividualAccessibility;
    public SectorIndividualAccessibility IndividualAccessibility => individualAccessibility;
    public NecessityDegree Necessity => necessity;
    // public float Probability => probability;

    /// <summary>
    /// May be set during the generation
    /// </summary>
    public int? GeneratedSectorId { get; private set; }

    public void AssignGeneratedSectorId(int generatedSectorId) {
        GeneratedSectorId = generatedSectorId;
    }
}
