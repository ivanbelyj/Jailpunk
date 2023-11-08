using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates data of maze structure and instantiates on the scene
/// </summary>
public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private GenerationData generationData;
    // [SerializeField]
    private List<IGenerationStage> generationStages;

    [SerializeField]
    private bool showLogMessages = false;

    [SerializeField]
    private bool generateMazeOnAwake = true;

    public void Awake() {
        InitGenerationStages();

        if (generateMazeOnAwake)
            CreateMaze();
    }

    private void InitGenerationStages() {
        generationStages = new List<IGenerationStage>() {
            GetComponent<BSPGeneration>(),
            GetComponent<CorridorsStructureGeneration>(),

            GetComponent<TestStructureStage>(),

            GetComponent<StructureToSchemeStage>(),
            GetComponent<CorridorsToScheme>(),
            
            GetComponent<DebugGUI>()
        };
        foreach (var stage in generationStages) {
            stage.Initialize(generationData);
        }
    }

    /// <summary>
    /// Generates and instantiates the maze
    /// </summary>
    public GenerationContext CreateMaze() {
        float totalStartTime = GetMsSinceStartup();

        // For adding debug marks only!
        mazeSchemeSingleton = new MazeScheme(generationData.MazeSize);
        MazeData initialMazeData = new MazeData() {
            Scheme = mazeSchemeSingleton
        };
        GenerationContext lastProcessed = new GenerationContext() {
            MazeData = initialMazeData,
            // Random = generationData.UseRandomSeed ?
            //     new System.Random()
            //     : new System.Random(generationData.Seed)
        };

        int seedToInitState = generationData.UseRandomSeed ?
            new System.Random().Next() :
            generationData.Seed;
        Random.InitState(seedToInitState);
        if (showLogMessages)
            Debug.Log("Maze generation seed: " + seedToInitState);
            
        foreach (var stage in generationStages) {
            if (stage.IncludeInGeneration) {
                lastProcessed = stage.ProcessMaze(lastProcessed);
            }
        }

        float elapsedMs = GetMsSinceStartup() - totalStartTime;
        if (showLogMessages)  
            Debug.Log($"Maze generation completed. Elapsed: {elapsedMs} ms");
        return lastProcessed;
    }

    private float GetMsSinceStartup() {
        return Time.realtimeSinceStartup * 1000;
    }

    /// <summary>
    /// Singleton for debug marking only
    /// </summary>
    private static MazeScheme mazeSchemeSingleton;

    public static void AddDebugMarkToScheme(
        Vector2Int pos, Color? color = null) {
        if (mazeSchemeSingleton == null) {
            Debug.LogWarning("There is no maze scheme to add a debug mark");
            return;
        }
        mazeSchemeSingleton.AddDebugMark(pos, color);
    }
}
