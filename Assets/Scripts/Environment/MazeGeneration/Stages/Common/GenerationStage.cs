using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenerationStage : MonoBehaviour, IGenerationStage
{
    [SerializeField]
    private bool includeInGeneration = true;
    public bool IncludeInGeneration {
        get => includeInGeneration;
        set => includeInGeneration = value;
    }

    private string stageName;

    public string StageName {
        get {
            stageName ??= GetType().Name;
            return stageName;
        }
    }
    
    public abstract GenerationContext ProcessMaze(GenerationContext context);
}
