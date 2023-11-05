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
    [SerializeField]
    private List<GenerationStage> generationStages;

    [Header("Debug")]
    [SerializeField]
    private bool showDebugMessages = false;

    [SerializeField]
    private bool generateMazeOnAwake = true;

    public void Awake() {
        InitGenerationStages();

        if (generateMazeOnAwake)
            CreateMaze();
    }

    private void InitGenerationStages() {
        // generationStages = new List<IGenerationStage>() {
        //     GetComponent<StructureGeneration>(),
        //     GetComponent<DebugGUI>()
        // };
        foreach (var stage in generationStages) {
            stage.Initialize(generationData);
        }
    }

    /// <summary>
    /// Generates and instantiates the maze
    /// </summary>
    public GenerationContext CreateMaze() {
        float totalStartTime = GetMsSinceStartup();

        MazeData initialMazeData = new MazeData() {
            Walls = new int[,] {
                {1, 1, 1},
                {1, 0, 1},
                {1, 1, 1}
            }
        };
        GenerationContext lastProcessed = new GenerationContext() {
            MazeData = initialMazeData,
            // Random = generationData.UseRandomSeed ?
            //     new System.Random()
            //     : new System.Random(generationData.Seed)
        };
        if (!generationData.UseRandomSeed)
            Random.InitState(generationData.Seed);
        foreach (var stage in generationStages) {
            if (stage.IncludeInGeneration) {
                lastProcessed = stage.ProcessMaze(lastProcessed);
            }
        }

        float elapsedMs = GetMsSinceStartup() - totalStartTime;
        if (showDebugMessages)  
            Debug.Log($"Maze generation completed. Elapsed: {elapsedMs} ms");
        return lastProcessed;
    }

    private float GetMsSinceStartup() {
        return Time.realtimeSinceStartup * 1000;
    }
}
