using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorsStructureGeneration : GenerationStage
{
    [SerializeField]
    public int corridorsBreadth = 3;

    [Header("Debug")]
    [SerializeField]
    private bool isDebug = false;
    public override void RunStage()
    {
        var generator = new CorridorsGenerator(isDebug);
        var corridors = new List<CorridorArea>();
        foreach (var (node1, node2) in context.GenerationData.RawCorridorsConnectivity
            .ConnectedPairsUnique()) {
            var generatedCorridors = generator
                .CreateCorridors(node1.Value, node2.Value,
                    corridorsBreadth);
            if (generatedCorridors == null)
                continue;
            corridors.AddRange(generatedCorridors);
        
        }

        context.GenerationData.Corridors = corridors;

        // Todo:
        // traverse corridors and map to maze scheme as new sectors
    }
}
