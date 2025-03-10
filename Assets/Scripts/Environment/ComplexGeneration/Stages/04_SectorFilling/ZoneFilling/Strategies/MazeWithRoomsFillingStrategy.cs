using System;
using System.Linq;

using static MazeWithRoomsGenerator;

public class MazeWithRoomsFillingStrategy : LayerTargetedZoneFillingStrategy
{
    private readonly MazeWithRoomsGenerator mazeGenerator;
    private readonly string mapObjectSchemaAddress;
    private CellType[,] mazeGrid;

    public MazeWithRoomsFillingStrategy(
        Func<GeneratedZone, TraverseRectFilter> traverseRectFilterFactory,
        LayerTargetedZoneFillingStrategyOptions options,
        string mapObjectSchemaAddress)
        : base(traverseRectFilterFactory, options)
    {
        mazeGenerator = new MazeWithRoomsGenerator();
        this.mapObjectSchemaAddress = mapObjectSchemaAddress;
    }

    public override void Apply(TraverseZoneFillingStrategyContext context)
    {
        mazeGrid = mazeGenerator.Generate(context.Area.Rect);
        base.Apply(context);
    }

    public override void FillPosition(
        TraverseZoneFillingStrategyPositionContext context,
        SchemePosition position,
        SchemeTile schemeTile)
    {
        var data = context.TraverseData;
        var area = context.Area;
        var traverseFilter = context.TraverseFilter;

        int gridX = data.x - area.Rect.position.x;
        int gridY = data.y - area.Rect.position.y;
        var cellType = mazeGrid[gridX, gridY];

        if (traverseFilter != null) {
            if (!data.IsFilterSatisfied(traverseFilter)) {
                return;
            }
        }

        ApplyCell(context, position, schemeTile, cellType);
    }

    private void ApplyCell(
        TraverseZoneFillingStrategyPositionContext context,
        SchemePosition position,
        SchemeTile schemeTile,
        CellType cellType)
    {
        switch (cellType)
        {
            case CellType.Wall:
                schemeTile.MapObjectAddress = mapObjectSchemaAddress;
                break;
        }
    }
}

// public class MazeWithRoomsFillingStrategyOld : ZoneFillingStrategyBase
// {
//     private readonly MazeWithRoomsGenerator mazeGenerator;
//     private readonly Func<GeneratedZone, TraverseRectFilter> traverseRectFilterFactory;
//     private readonly string targetLayerName;
//     private readonly string mapObjectSchemaAddress;

//     public MazeWithRoomsFillingStrategyOld(
//         string targetLayerName,
//         string mapObjectSchemaAddress,
//         Func<GeneratedZone, TraverseRectFilter> traverseRectFilterFactory)
//     {
//         mazeGenerator = new MazeWithRoomsGenerator();
//         this.traverseRectFilterFactory = traverseRectFilterFactory;
//         this.targetLayerName = targetLayerName;
//         this.mapObjectSchemaAddress = mapObjectSchemaAddress;
//     }

//     public override void Apply(GeneratedZone generatedZone, GenerationContext context)
//     {
//         GenerateMaze(generatedZone, context);
//     }

//     private void GenerateMaze(GeneratedZone zone, GenerationContext context)
//     {
//         var area = GetSchemeArea(zone, context);
//         if (area == null) {
//             return;
//         }

//         var mazeGrid = mazeGenerator.Generate(area.Rect);

//         var traverseFilter = traverseRectFilterFactory?.Invoke(zone);
//         TraverseRectUtils.TraverseRect(area.Rect, (data) => {
//             int gridX = data.x - area.Rect.position.x;
//             int gridY = data.y - area.Rect.position.y;
//             var cellType = mazeGrid[gridX, gridY];

//             if (traverseFilter != null) {
//                 if (!data.IsFilterSatisfied(traverseFilter)) {
//                     return;
//                 }
//             }

//             ApplyCell(context, data.x, data.y, cellType);
//         });
//     }

//     private SchemeArea GetSchemeArea(GeneratedZone zone, GenerationContext context)
//     {
//         return context.ComplexData.Scheme.Areas.FirstOrDefault(a => a.Id == zone.SchemeAreaId);
//     }

//     private void ApplyCell(GenerationContext context, int x, int y, CellType cellType)
//     {
//         var tile = context.ComplexData.Scheme.GetTileByPos(x, y);
//         switch (cellType)
//         {
//             case CellType.Wall:
//                 PlaceWall(tile, x, y);
//                 break;
//         }
//     }

//     private void PlaceWall(SchemePosition position, int x, int y)
//     {
//         if (position.Type == SchemePositionType.Floor)
//         {
//             position.Type = SchemePositionType.Wall;

//             var layer = string.IsNullOrWhiteSpace(targetLayerName)
//                 ? position.GetLayerByName(targetLayerName)
//                 : null;
//             if (layer == null) {
//                 layer = new SchemeTile();
//                 if (!string.IsNullOrWhiteSpace(targetLayerName)) {
//                     layer.LayerName = targetLayerName;
//                 }
//                 position.Layers.Add(layer);
//             }
//             layer.MapObjectAddress = mapObjectSchemaAddress;
//         }
//     }
// }
