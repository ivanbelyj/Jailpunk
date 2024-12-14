using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

/// <summary>
/// Generates data of maze structure and instantiates on the scene
/// </summary>
public class MazeGenerator : MonoBehaviour
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
    
    /// <summary>
    /// Static for debug marking
    /// </summary>
    private static MazeScheme mazeScheme;

    private string lastGeneratedRootGameObjectName;
    private GenerationRequest lastGenerationRequest;

    /// <summary>
    /// Generates and instantiates the maze
    /// </summary>
    public GenerationContext CreateMaze(
        GenerationRequest request,
        string rootGameObjectName = "MazeGrid") {
        float totalStartTime = GetMsSinceStartup();

        lastGenerationRequest = request;
        lastGeneratedRootGameObjectName = rootGameObjectName;

        var context = CreateInitialGenerationContext(rootGameObjectName, request);
        InitSeed(request.Parameters);
        context = RunGenerationStages(context);

        float elapsedMs = GetMsSinceStartup() - totalStartTime;
        if (showLogMessages)   {
            Debug.Log($"Maze generation completed. Elapsed: {elapsedMs} ms");
        }

        return context;
    }

    public void Regenerate(string rootGameObjectName) {
        if (lastGenerationRequest == null) {
            Debug.LogError($"Cannot regenerate: no previous generation request defined");
            return;
        }
        Destroy(GameObject.Find(lastGeneratedRootGameObjectName));
        CreateMaze(lastGenerationRequest, rootGameObjectName ?? lastGeneratedRootGameObjectName);
    }

    #region Debug
    public static void AddDebugMarkToScheme(Vector2Int pos, Color? color = null) {
        if (mazeScheme == null) {
            Debug.LogWarning("There is no maze scheme to add a debug mark");
            return;
        }
        mazeScheme.AddDebugMark(pos, color);
    }

    public static void AddDebugSectorColor(int sectorId, Color color) {
        if (mazeScheme == null) {
            Debug.LogWarning("There is no maze scheme to add a debug sector color");
            return;
        }
        mazeScheme.AddDebugSectorColor(sectorId, color);
    }
    #endregion

    private void Awake() {
        SetGenerationStages();
    }

    private List<GenerationStage> GetDefaultAttachedGenerationStages() {
        return new List<GenerationStage>() {
            GetComponent<BSPGeneration>(),
            GetComponent<CorridorsStructureGeneration>(),

            GetComponent<TestStructureStage>(),

            GetComponent<StructureToSchemeStage>(),
            GetComponent<CorridorsToScheme>(),

            GetComponent<SectorsConnectivityProcessing>(),

            GetComponent<RequestedSectorsAllocation>(),
            GetComponent<SectorPlanning>(),
            GetComponent<PassagesPlanning>(),

            GetComponent<MazeBuilding>(),
            
            GetComponent<DebugGUI>()
        };
    }

    private void SetGenerationStages() {
        if (!useCustomGenerationStages) {
            generationStages = GetDefaultAttachedGenerationStages();
        }
    }

    private GenerationContext CreateInitialGenerationContext(
        string rootGameObjectName,
        GenerationRequest generationRequest)
    {
        mazeScheme = new(generationRequest.Parameters.MazeSize);
        var initialMazeData = new MazeData() {
            Scheme = mazeScheme
        };

        return new GenerationContext() {
            RootGameObjectName = rootGameObjectName,
            MazeData = initialMazeData,
            Settings = generationSettings,
            Request = generationRequest
            // Random = generationData.UseRandomSeed ?
            //     new System.Random()
            //     : new System.Random(generationData.Seed)
        };
    }

    private void InitSeed(GenerationParameters parameters) {
        int seedToInitState = parameters.UseRandomSeed ?
            new System.Random().Next() :
            parameters.Seed;
        
        Random.InitState(seedToInitState);
        if (showLogMessages) {
            Debug.Log("Maze generation seed: " + seedToInitState);
        }
    }

    private GenerationContext RunGenerationStages(GenerationContext context) {
        foreach (var stage in generationStages) {
            if (stage.IncludeInGeneration) {
                context = stage.ProcessMaze(context);
            }
        }
        return context;
    }

    private float GetMsSinceStartup() {
        return Time.realtimeSinceStartup * 100;
    }
}
