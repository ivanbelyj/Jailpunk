using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Applies data generated on the <i>base</i> structure generation stage
/// to the maze scheme.
/// </summary>
public class StructureToSchemeStage : GenerationStage
{
    public override void ProcessMaze()
    {
        var generatedSectors = CreateAndApplySectors(context);
        context.GeneratedSectors = generatedSectors;
        
        // if (context.Corridors != null) {
        //     foreach (CorridorArea corridor in context.Corridors) {
        //         StructureUtils.ApplyCorridor(scheme, corridor);
        //     }
        // }
    }

    private List<GeneratedSectorInfo> CreateAndApplySectors(GenerationContext context) {
        var generatedSectors = new List<GeneratedSectorInfo>();
        var scheme = context.MazeData.Scheme;
        
        for (int i = 0; i < context.SectorRects.Count; i++) {
            RectArea sectorRect = context.SectorRects[i];
            var sectorInfo = new GeneratedSectorInfo() {
                Id = context.IdGenerator.NewSectorId(),
                RectArea = sectorRect
            };
            generatedSectors.Add(sectorInfo);
            ApplySector(scheme, sectorRect, sectorInfo.Id);
        }
        return generatedSectors;
    }

    private void ApplySector(MazeScheme scheme, RectArea rect, int sectorId) {
        StructureUtils.TraverseRect(rect.Rect, (x, y, isBorder) => {
            SchemeTile tile = scheme.GetTileByPos(x, y);
            tile.SectorId = sectorId;
            tile.TileType = isBorder ?
                TileType.LoadBearingWall : TileType.Floor;
        });
    }
}
