using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGenerationStage
{
    void Initialize(GenerationData generationData);
    MazeData ProcessMaze(MazeData mazeData);
    string StageName { get; }
    bool IncludeInGeneration { get; }
}
