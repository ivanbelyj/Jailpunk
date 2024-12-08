using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SectorRoomPlanningStrategy : ISectorPlanningStrategy
{
    public void PlanSector(SectorInfo sector, GenerationContext context)
    {
        var roomBlocksGenerator = new RoomBlocksGenerator(context.Settings);
        
        roomBlocksGenerator.GenerateAndApply(
            context.MazeData.Scheme,
            sector.RectArea.Rect,
            context.Settings.sectorBSPOptions);
    }
}
