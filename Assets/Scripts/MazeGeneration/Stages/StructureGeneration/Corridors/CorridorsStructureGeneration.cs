using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorsStructureGeneration : GenerationStage
{
    [SerializeField]
    public int corridorsBreadth = 3;
    public override GenerationContext ProcessMaze(GenerationContext context)
    {
        var generator = new CorridorsGenerator();
        var corridors = new List<CorridorArea>();
        foreach (var (node1, node2) in context.RawCorridorsConnectivity
            .ConnectedPairsUnique()) {
            corridors.AddRange(generator
                .CreateCorridors(node1.Value, node2.Value,
                    corridorsBreadth));
        }

        context.Corridors = corridors;

        // Todo:
        // traverse corridors and map to maze scheme as new sectors
        
        return context;
    }
}
