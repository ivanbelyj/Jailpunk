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
    private List<IGenerationStage> generationStages;

    [Header("Debug")]
    [SerializeField]
    private bool showDebugMessages = false;

    public void Awake() {
        InitGenerationStages();
    }

    private void InitGenerationStages() {
        generationStages = new List<IGenerationStage>();
    }

    /// <summary>
    /// Generates and instantiates the maze
    /// </summary>
    public MazeData CreateMaze() {
        float totalStartTime = GetMsSinceStartup();

        MazeData initialMazeData = new MazeData() {
        };
        MazeData lastProcessed = initialMazeData;
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
