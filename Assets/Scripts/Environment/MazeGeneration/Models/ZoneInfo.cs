using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sector zone is a space with dedicated responsibility, functionally segregated
/// from other. Defined by the one of sector responsibilities, generated
/// on the level, higher than generation stages
/// </summary>
[System.Serializable]
public class ZoneInfo : IAllocatableArea
{
    [SerializeField]
    private int zoneGroupId;

    [SerializeField]
    private bool useIndividualAccessibility = true;

    [SerializeField]
    private AreaIndividualAccessibility individualAccessibility = AreaIndividualAccessibility.High;

    [SerializeField]
    private NecessityDegree necessity = NecessityDegree.Required;

    public int ZoneGroupId => zoneGroupId;
    public bool UseIndividualAccessibility => useIndividualAccessibility;
    public AreaIndividualAccessibility IndividualAccessibility => individualAccessibility;
    public NecessityDegree Necessity => necessity;

    /// <summary>
    /// May be set during the generation
    /// </summary>
    public int? GeneratedZoneId { get; private set; }

    int IAllocatableArea.AreaGroupId => ZoneGroupId;
    int? IAllocatableArea.GeneratedAreaId => GeneratedZoneId;

    public void AssignGeneratedSectorId(int generatedZoneId) {
        GeneratedZoneId = generatedZoneId;
    }

    void IAllocatableArea.AssignGeneratedAreaId(int generatedZoneId)
    {
        AssignGeneratedSectorId(generatedZoneId);
    }
}
