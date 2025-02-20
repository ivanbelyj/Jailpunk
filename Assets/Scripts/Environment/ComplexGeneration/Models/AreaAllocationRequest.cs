using UnityEngine;

[System.Serializable]
public class AreaAllocationRequest {
    [SerializeField]
    private int id;

    [SerializeField]
    private int areaGroupId;

    [SerializeField]
    private bool useIndividualAccessibility;

    [SerializeField]
    private AreaIndividualAccessibility individualAccessibility;

    [SerializeField]
    private NecessityDegree necessity;

    public int Id { get => id; set => id = value; }
    public int AreaGroupId { get => areaGroupId; set => areaGroupId = value; }
    public bool UseIndividualAccessibility {
        get => useIndividualAccessibility;
        set => useIndividualAccessibility = value;
    }
    public AreaIndividualAccessibility IndividualAccessibility {
        get => individualAccessibility;
        set => individualAccessibility = value;
    }
    public NecessityDegree Necessity { get => necessity; set => necessity = value; }

    public AreaAllocationRequest(int id, int areaGroupId)
    {
        this.id = id;
        this.areaGroupId = areaGroupId;
    }
}
