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
    public override void RunStage()
    {
        var generatedSectors = CreateAndApplySectors(context);
        GenerationData.GeneratedSectors = generatedSectors;
        
        // if (context.Corridors != null) {
        //     foreach (CorridorArea corridor in context.Corridors) {
        //         StructureUtils.ApplyCorridor(scheme, corridor);
        //     }
        // }
    }

    private List<GeneratedSectorInfo> CreateAndApplySectors(GenerationContext context) {
        var generatedSectors = new List<GeneratedSectorInfo>();
        var scheme = context.ComplexData.Scheme;
        
        for (int i = 0; i < GenerationData.SectorRects.Count; i++) {
            RectArea sectorRect = GenerationData.SectorRects[i];
            var sectorInfo = new GeneratedSectorInfo() {
                Id = idGenerator.NewSectorId(),
                RectArea = sectorRect
            };
            generatedSectors.Add(sectorInfo);
            ApplySector(scheme, sectorRect, sectorInfo.Id);
        }
        return generatedSectors;
    }

    private void ApplySector(ComplexScheme scheme, RectArea rect, int sectorId) {
        TraverseRectUtils.TraverseRect(rect.Rect, (data) => {
            bool isBorder = data.isBorder;
            int x = data.x;
            int y = data.y;
            SchemePosition tile = scheme.GetTileByPos(x, y);
            tile.SectorId = sectorId;
            tile.Type = isBorder ?
                SchemePositionType.LoadBearingWall : SchemePositionType.Floor;
        });
    }
}
