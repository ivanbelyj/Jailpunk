using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGenerationStage
{
    GenerationContext ProcessMaze(GenerationContext mazeData);
    string StageName { get; }
    bool IncludeInGeneration { get; }
}
