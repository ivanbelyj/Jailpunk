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
            if (stageName == null) {
                stageName = GetType().Name;
            }
            return stageName;
        }
    }

    protected GenerationData generationData;
    
    public virtual void Initialize(GenerationData generationData)
    {
        this.generationData = generationData;
    }
    
    public abstract GenerationContext ProcessMaze(GenerationContext context);
}
