using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGenerationStage
{
    void ProcessMaze();
    string StageName { get; }
    bool IncludeInGeneration { get; }
}
