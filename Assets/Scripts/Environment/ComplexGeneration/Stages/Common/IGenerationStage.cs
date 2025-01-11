using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGenerationStage
{
    void RunStage();
    string StageName { get; }
    bool IncludeInGeneration { get; }
}
