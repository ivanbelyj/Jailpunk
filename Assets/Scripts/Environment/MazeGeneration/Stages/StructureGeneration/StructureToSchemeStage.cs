using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Applies data generated on the <i>base</i> structure generation stage
/// to the maze scheme.
/// Structure generation stage (SGS) is some stage operating with 
/// with vector or rect space representation,
/// and not with a maze scheme (map).
/// Usually such a stage precedes to a stage operating with the maze scheme
/// </summary>
public class StructureToSchemeStage : GenerationStage
{
    public override GenerationContext ProcessMaze(GenerationContext context)
    {
        var scheme = context.MazeData.Scheme;
        
        for (int i = 0; i < context.Sectors.Count; i++) {
            RectArea sectorRoom = context.Sectors[i];
            StructureUtils.ApplyNewSector(scheme, sectorRoom, i + 1);
        }
        // if (context.Corridors != null) {
        //     foreach (CorridorArea corridor in context.Corridors) {
        //         StructureUtils.ApplyCorridor(scheme, corridor);
        //     }
        // }
        
        return context;
    }
}
