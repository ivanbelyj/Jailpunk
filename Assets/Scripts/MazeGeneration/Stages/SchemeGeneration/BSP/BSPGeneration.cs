using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSPGeneration : GenerationStage
{
    // [SerializeField]
    // [Tooltip("Controls frequency of splitting leaves that are met size requirements")]
    // private float splitLeafProbability = 0.75f;

    // [SerializeField]
    // [Tooltip("Used to determine whether the leaf will split horizontally or vertically")]
    // private float minSplitRatio = 1.25f;
    [SerializeField]
    [Tooltip("Options used for BSP maze structure generation. "
        + "Some options will be overriden in the generation")]
    private BSPGenerationOptions options;

    public override GenerationContext ProcessMaze(GenerationContext genContext)
    {
        BSPGenerator generator = new BSPGenerator();

        options.rootLeafSize = generationData.MazeSize;
        options.minLeafSize = generationData.MinSectorSize;
        options.maxLeafSize = generationData.MaxSectorSize;

        genContext.BSPLeaves = generator.GenerateBSPLeaves(
            options
            // generationData.MazeSize,
            // generationData.MinSectorSize,
            // generationData.MaxSectorSize,
            // splitLeafProbability,
            // minSplitRatio
        );
        return genContext;
    }
}
