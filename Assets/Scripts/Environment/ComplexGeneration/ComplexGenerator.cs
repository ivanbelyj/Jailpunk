using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

/// <summary>
/// Generates data of complex structure and instantiates on the scene
/// </summary>
public class ComplexGenerator : MonoBehaviour
{
    [SerializeField]
    private GenerationSettings generationSettings;

    [Tooltip("If true then generation stages must be defined. Otherwise attached components will be used")]
    [SerializeField]
    private bool useCustomGenerationStages;

    [SerializeField]
    private List<GenerationStage> generationStages;

    [SerializeField]
    private bool showLogMessages = false;

    private readonly IdGenerator idGenerator = IdGenerator.Instance;
    
    /// <summary>
    /// Static for debug marking
    /// </summary>
    private static ComplexScheme complexScheme;

    private string lastGeneratedRootGameObjectName;
    private GenerationRequest lastGenerationRequest;

    /// <summary>
    /// Generates and instantiates the complex
    /// </summary>
    public GenerationContext CreateComplex(
        GenerationRequest request,
        string rootGameObjectName = "ComplexGrid")
    {
        float totalStartTime = GetMsSinceStartup();

        lastGenerationRequest = request;
        lastGeneratedRootGameObjectName = rootGameObjectName;

        var context = CreateInitialGenerationContext(rootGameObjectName, request);
        InitSeed(request.Parameters);
        RunGenerationStages(context, idGenerator);

        float elapsedMs = GetMsSinceStartup() - totalStartTime;
        if (showLogMessages)   {
            Debug.Log($"Complex generation completed. Elapsed: {elapsedMs} ms");
        }

        return context;
    }

    public void Regenerate(string rootGameObjectName)
    {
        if (lastGenerationRequest == null) {
            Debug.LogError($"Cannot regenerate: no previous generation request defined");
            return;
        }
        Destroy(GameObject.Find(lastGeneratedRootGameObjectName));
        CreateComplex(lastGenerationRequest, rootGameObjectName ?? lastGeneratedRootGameObjectName);
    }

    #region Debug
    public static void AddDebugMarkToScheme(Vector2Int pos, Color? color = null)
    {
        if (complexScheme == null) {
            Debug.LogWarning("There is no complex scheme to add a debug mark");
            return;
        }
        complexScheme.AddDebugMark(pos, color);
    }

    public static void AddDebugSectorColor(int sectorId, Color color)
    {
        if (complexScheme == null) {
            Debug.LogWarning("There is no complex scheme to add a debug sector color");
            return;
        }
        complexScheme.AddDebugSectorColor(sectorId, color);
    }

    public static void AddDebugAreaColor(int areaId, Color color)
    {
        if (complexScheme == null) {
            Debug.LogWarning("There is no complex scheme to add a debug sector color");
            return;
        }
        complexScheme.AddDebugAreaColor(areaId, color);
    }
    #endregion

    private void Awake()
    {
        SetGenerationStages();
    }

    private List<GenerationStage> GetDefaultAttachedGenerationStages()
    {
        return new List<GenerationStage>() {
            GetComponent<SectorSeparation>(),
            GetComponent<CorridorsStructureGeneration>(),

            GetComponent<TestStructureStage>(),

            GetComponent<StructureToSchemeStage>(),
            GetComponent<CorridorsToScheme>(),

            GetComponent<SectorsConnectivityProcessing>(),

            GetComponent<RequestedSectorsAllocation>(),
            GetComponent<SectorPlanning>(),
            GetComponent<PassagesPlanning>(),
            GetComponent<ZoneAllocation>(),
            GetComponent<ZoneFilling>(),

            GetComponent<ComplexBuilding>(),
            
            GetComponent<DebugGUI>()
        };
    }

    private void SetGenerationStages()
    {
        if (!useCustomGenerationStages) {
            generationStages = GetDefaultAttachedGenerationStages();
        }
    }

    private GenerationContext CreateInitialGenerationContext(
        string rootGameObjectName,
        GenerationRequest generationRequest)
    {
        complexScheme = new(generationRequest.Parameters.MazeSize);
        var initialComplexData = new ComplexData() {
            Scheme = complexScheme
        };

        return new GenerationContext() {
            RootGameObjectName = rootGameObjectName,
            ComplexData = initialComplexData,
            Settings = generationSettings,
            Request = generationRequest
            // Random = generationData.UseRandomSeed ?
            //     new System.Random()
            //     : new System.Random(generationData.Seed)
        };
    }

    private void InitSeed(GenerationParameters parameters)
    {
        int seedToInitState = parameters.UseRandomSeed ?
            new System.Random().Next() :
            parameters.Seed;
        
        Random.InitState(seedToInitState);
        if (showLogMessages) {
            Debug.Log("Complex generation seed: " + seedToInitState);
        }
    }

    private void RunGenerationStages(
        GenerationContext context,
        IdGenerator idGenerator)
    {
        foreach (var stage in generationStages) {
            if (stage.IncludeInGeneration) {
                stage.Initialize(context, idGenerator);
                stage.RunStage();
            }
        }
    }

    private float GetMsSinceStartup()
    {
        return Time.realtimeSinceStartup * 100;
    }
}
