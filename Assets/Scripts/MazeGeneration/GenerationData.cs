using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parameters of the maze generation process
/// </summary>
[System.Serializable]
public class GenerationData
{
    [SerializeField]
    private int seed;
    public int Seed { get => seed; private set => seed = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
