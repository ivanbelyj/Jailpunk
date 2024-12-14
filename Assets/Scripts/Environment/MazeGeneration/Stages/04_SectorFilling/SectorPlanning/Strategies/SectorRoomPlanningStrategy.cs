using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SectorRoomPlanningStrategy : ISectorPlanningStrategy
{
    public void PlanSector(GeneratedSectorInfo sector, GenerationContext context)
    {
        var roomBlocksGenerator = new RoomBlocksGenerator(
            context.Settings,
            context.IdGenerator);
        
        roomBlocksGenerator.GenerateAndApply(
            context.MazeData.Scheme,
            sector.RectArea.Rect,
            context.Settings.sectorBSPOptions);
    }
}
