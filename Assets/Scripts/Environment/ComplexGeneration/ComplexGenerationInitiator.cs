using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplexGenerationInitiator : MonoBehaviour
{
    [SerializeField]
    private bool generateOnStart = true;

    [SerializeField]
    private GenerationRequest generationRequest;

    public void Generate(GenerationRequest generationRequest = null) {
        if (generationRequest != null) {
            this.generationRequest = generationRequest;
        }
        GetGenerator().CreateComplex(this.generationRequest);
    }

    private void Start()
    {
        if (generateOnStart) {
            Generate();
        }
    }

    private ComplexGenerator GetGenerator() {
        return FindAnyObjectByType<ComplexGenerator>();
    }
}
