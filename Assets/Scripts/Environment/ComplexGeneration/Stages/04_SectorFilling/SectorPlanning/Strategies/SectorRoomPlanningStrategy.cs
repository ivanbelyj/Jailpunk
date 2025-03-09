using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SectorRoomPlanningStrategy : SectorPlanningStrategyBase
{
    private readonly IdGenerator idGenerator;

    public SectorRoomPlanningStrategy(IdGenerator idGenerator)
    {
        this.idGenerator = idGenerator;
    }
    
    public override List<SchemeArea> PlanSector(
        GeneratedSectorInfo sector,
        GenerationContext context)
    {
        var roomBlocksGenerator = new RoomBlocksGenerator(context.Settings);
        
        var bspNodes = roomBlocksGenerator.GenerateBSPNodes(
            context.ComplexData.Scheme,
            sector.RectArea.Rect,
            context.Settings.sectorBSPOptions);
        
        var applyAreaHelper = new ApplyAreaHelper(idGenerator);
        return applyAreaHelper.ToSchemeAreas(bspNodes);
    }
}
