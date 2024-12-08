using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerationInitiator : MonoBehaviour
{
    [SerializeField]
    private bool generateOnStart = true;

    [SerializeField]
    private GenerationRequest generationRequest;

    public void Generate(GenerationRequest generationRequest = null) {
        if (generationRequest != null) {
            this.generationRequest = generationRequest;
        }
        GetGenerator().CreateMaze(this.generationRequest);
    }

    private void Start()
    {
        if (generateOnStart) {
            Generate();
        }
    }

    private MazeGenerator GetGenerator() {
        return FindObjectOfType<MazeGenerator>();
    }
}
