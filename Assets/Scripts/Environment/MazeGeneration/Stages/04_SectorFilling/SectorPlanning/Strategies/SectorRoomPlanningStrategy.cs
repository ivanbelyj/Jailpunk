using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SectorRoomPlanningStrategy : ISectorPlanningStrategy
{
    public List<SchemeArea> PlanSector(
        GeneratedSectorInfo sector,
        GenerationContext context)
    {
        var roomBlocksGenerator = new RoomBlocksGenerator(context.Settings);
        
        var bspNodes = roomBlocksGenerator.GenerateBSPNodes(
            context.MazeData.Scheme,
            sector.RectArea.Rect,
            context.Settings.sectorBSPOptions);
        
        var applyAreaHelper = new ApplyAreaHelper(context.IdGenerator);
        return applyAreaHelper.ToSchemeAreas(bspNodes);
    }
}
