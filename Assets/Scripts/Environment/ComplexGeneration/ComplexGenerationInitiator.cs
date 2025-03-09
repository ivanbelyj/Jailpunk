using TigerForge;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ComplexGenerationInitiator : MonoBehaviour
{
    public enum GenerationInitiatorBehaviourOnStart {
        GenerateWhenReady,
        [Tooltip("Allows to use manually created grid without performing generation")]
        EmitGridReadyEventOnStart,
        DoNothingOnStart
    }

    [SerializeField]
    private GenerationInitiatorBehaviourOnStart behaviourOnStart
        = GenerationInitiatorBehaviourOnStart.GenerateWhenReady;

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
            case GenerationInitiatorBehaviourOnStart.GenerateWhenReady:
                EventManager.StartListening(
                    EventConstants.ComplexGenerationAssetsLoaded,
                    () => Generate());
                break;
            case GenerationInitiatorBehaviourOnStart.EmitGridReadyEventOnStart:
                EmitGridReadyEventForManualGridData();
                break;
            case GenerationInitiatorBehaviourOnStart.DoNothingOnStart:
                // Do nothing
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
            .AddSectorGroup()
                .AddSector()
                    .AddZoneGroup()
                        .AddZones(5)
                    .AddZoneGroup()
                        .AddZones(5)
                    .AddZoneGroup()
                        .AddZones(5)
                    .AddZoneGroup()
                        .AddZones(5)
                    .AddZoneGroup(alienateAll: true)
                        .AddZones(5)
                .AddSector()
                    .AddZoneGroup()
                        .AddZones(3)
                    .AddZoneGroup()
                        .AddZones(3)
                .AddSector()
                    .AddZones(2)
            .AddSectorGroup(alienateAll: true)
                .AddSector()
                    .AddZones(3)
            .Build();
        
        generationRequest.SectorGroups = groups;
        generationRequest.RequestedSectors = sectors;
    }

    private ComplexGenerator GetGenerator() {
        return FindAnyObjectByType<ComplexGenerator>();
    }
}
