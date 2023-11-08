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
            var corridorsGenRes = generator
                .CreateCorridors(node1.Value, node2.Value,
                    corridorsBreadth);
            if (corridorsGenRes == null)
                continue;
            corridors.AddRange(corridorsGenRes.Value.Item1);

            void AddMark(Vector2Int pos, Color col) {
                context.MazeData.Scheme.AddDebugMark(pos, col);
            }
            
            AddMark(corridorsGenRes.Value.Item2.Item1, Color.red);
            AddMark(corridorsGenRes.Value.Item2.Item2, Color.green);
        }

        context.Corridors = corridors;

        // Todo:
        // traverse corridors and map to maze scheme as new sectors
        
        return context;
    }
}
