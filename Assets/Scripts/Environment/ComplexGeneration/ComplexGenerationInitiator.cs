using TigerForge;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ComplexGenerationInitiator : MonoBehaviour
{
    public enum GenerationInitiatorBehaviourOnStart {
        GenerateOnStart,
        [Tooltip("Allows to use manually created grid without performing generation")]
        EmitGridReadyEventOnStart,
        DoNothingOnStart
    }

    [SerializeField]
    private GenerationInitiatorBehaviourOnStart behaviourOnStart
        = GenerationInitiatorBehaviourOnStart.GenerateOnStart;

    [SerializeField]
    private GenerationRequest generationRequest;

    [Header("Manual grid data")]
    [SerializeField]
    private GameObject gridGameObject;

    [SerializeField]
    private Tilemap visionObstacleTilemap;

    [SerializeField]
    private Tilemap obstacleTilemap;

    public void Generate(GenerationRequest generationRequest = null) {
        if (generationRequest != null) {
            this.generationRequest = generationRequest;
        }
        SetRequestedSectors(this.generationRequest);

        var generationContext = GetGenerator().CreateComplex(this.generationRequest);
        EmitGridReadyEvent(generationContext.GenerationData.InstantiatedComplexData);
    }

    private void Start()
    {
        switch (behaviourOnStart) {
            case GenerationInitiatorBehaviourOnStart.GenerateOnStart:
                Generate();
                break;
            case GenerationInitiatorBehaviourOnStart.EmitGridReadyEventOnStart:
                EmitGridReadyEventForManualGridData();
                break;
        }
    }

    private void EmitGridReadyEventForManualGridData() {
        if (gridGameObject == null
            || obstacleTilemap == null
            || visionObstacleTilemap == null)
        {
            Debug.LogError(
                "Manual grid data is not fully defined to use " +
                $"{nameof(GenerationInitiatorBehaviourOnStart.EmitGridReadyEventOnStart)} " +
                "start behaviour");
        }
        EmitGridReadyEvent(new() {
            GridGameObject = gridGameObject,
            ObstacleTilemap = obstacleTilemap,
            VisionObstacleTilemap = visionObstacleTilemap
        });
    }

    private void EmitGridReadyEvent(InstantiatedComplexData instantiatedComplexData) {
        EventManager.EmitEventData(
            EventConstants.GridReady,
            new GridInstantiatedEventData() {
                InstantiatedComplexData = instantiatedComplexData
            });
    }

    private void SetRequestedSectors(GenerationRequest generationRequest) {
        var sectorRequestsBuilder = new ComplexSectorRequestsBuilder();
        var (sectors, groups) = sectorRequestsBuilder
            .AddGroup()
                .AddSector()
                    .AddZones(3)
                .AddSector()
                .AddSector()
            .AddGroup(alienateAll: true)
                .AddSector()
                    .AddZones(1)
            .Build();
        
        generationRequest.SectorGroups = groups;
        generationRequest.RequestedSectors = sectors;
    }

    private ComplexGenerator GetGenerator() {
        return FindAnyObjectByType<ComplexGenerator>();
    }
}
