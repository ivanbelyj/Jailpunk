using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sector requirements defined on higher level by the plot necessity
/// </summary>
[System.Serializable]
public class SectorInfo : IAllocatableArea
{
    [SerializeField]
    private int sectorGroupId;

    [SerializeField]
    private bool useIndividualAccessibility = true;

    [SerializeField]
    private AreaIndividualAccessibility individualAccessibility = AreaIndividualAccessibility.High;

    [SerializeField]
    private NecessityDegree necessity = NecessityDegree.Required;

    [SerializeField]
    private List<AllocatableAreaGroup> zoneGroups;

    [SerializeField]
    private List<ZoneInfo> zones;

    public int SectorGroupId => sectorGroupId;
    public bool UseIndividualAccessibility => useIndividualAccessibility;
    public AreaIndividualAccessibility IndividualAccessibility => individualAccessibility;
    public NecessityDegree Necessity => necessity;

    /// <summary>
    /// Zones requested for the sector
    /// </summary>
    public List<ZoneInfo> Zones => zones;

    public List<AllocatableAreaGroup> ZoneGroups => zoneGroups;

    /// <summary>
    /// May be set during the generation
    /// </summary>
    public int? GeneratedSectorId { get; private set; }

    int IAllocatableArea.AreaGroupId => SectorGroupId;
    int? IAllocatableArea.GeneratedAreaId => GeneratedSectorId;

    public void AssignGeneratedSectorId(int generatedSectorId) {
        GeneratedSectorId = generatedSectorId;
    }

    void IAllocatableArea.AssignGeneratedAreaId(int generatedSectorId)
    {
        AssignGeneratedSectorId(generatedSectorId);
    }
}
