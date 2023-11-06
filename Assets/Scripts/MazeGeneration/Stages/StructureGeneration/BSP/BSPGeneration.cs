using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Base maze structure generation stage using BSP
/// </summary>
public class BSPGeneration : GenerationStage
{
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

        var bspLeaves = generator.GenerateBSPLeaves(options);

        genContext.Sectors = bspLeaves
            .Select(leaf => leaf.room)
            .Where(room => room != null)
            .ToList();

        var corridorsAll = new List<CorridorSpace>();
        foreach (BSPLeaf leaf in bspLeaves) {
            if (leaf.corridors != null) {
                foreach (CorridorSpace corridor in leaf.corridors) {
                    corridorsAll.Add(corridor);
                }
            }
        };
        genContext.Corridors = corridorsAll;

        return genContext;
    }
}
