using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static MazeWithRoomsGenerator;

public class MazeWithRoomsFillingStrategy : ZoneFillingStrategyBase
{
    private readonly MazeWithRoomsGenerator mazeGenerator;
    private readonly Func<GeneratedZone, TraverseRectFilter> traverseRectFilterFactory;

    public MazeWithRoomsFillingStrategy(Func<GeneratedZone, TraverseRectFilter> traverseRectFilterFactory)
    {
        mazeGenerator = new MazeWithRoomsGenerator();
        this.traverseRectFilterFactory = traverseRectFilterFactory;
    }

    public override void Apply(GeneratedZone generatedZone, GenerationContext context)
    {
        GenerateMaze(generatedZone, context);
    }

    private void GenerateMaze(GeneratedZone zone, GenerationContext context)
    {
        var area = GetSchemeArea(zone, context);
        if (area == null) {
            return;
        }

        var mazeGrid = mazeGenerator.Generate(area.Rect);

        var traverseFilter = traverseRectFilterFactory?.Invoke(zone);
        TraverseRectUtils.TraverseRect(area.Rect, (data) => {
            int gridX = data.x - area.Rect.position.x;
            int gridY = data.y - area.Rect.position.y;
            var cellType = mazeGrid[gridX, gridY];

            if (traverseFilter != null) {
                if (!data.IsFilterSatisfied(traverseFilter)) {
                    return;
                }
            }

            ApplyCell(context, data.x, data.y, cellType);
        });
    }

    private SchemeArea GetSchemeArea(GeneratedZone zone, GenerationContext context)
    {
        return context.ComplexData.Scheme.Areas.FirstOrDefault(a => a.Id == zone.SchemeAreaId);
    }

    private void ApplyCell(GenerationContext context, int x, int y, CellType cellType)
    {
        var tile = context.ComplexData.Scheme.GetTileByPos(x, y);
        switch (cellType)
        {
            case CellType.Wall:
                PlaceWall(tile, x, y);
                break;
            // case CellType.Path:
            //     PlacePath(tile, x, y);
            //     break;
            // case CellType.Room:
            //     PlaceRoom(context, x, y);
            //     break;
            // case CellType.RoomEntrance:
            //     PlaceRoomEntrance(context, x, y);
            //     break;
        }
    }

    private void PlaceWall(SchemePosition tile, int x, int y)
    {
        if (tile.Type == SchemePositionType.Floor) {
            tile.Type = SchemePositionType.Wall;
        }
        // MazeGenerator.AddDebugMarkToScheme(new Vector2Int(x, y), Color.black);
    }

    // private void PlacePath(SchemeTile tile, int x, int y)
    // {
    //     MazeGenerator.AddDebugMarkToScheme(new Vector2Int(x, y), Color.white);
    // }

    // private void PlaceRoom(GenerationContext context, int x, int y)
    // {
    //     MazeGenerator.AddDebugMarkToScheme(new Vector2Int(x, y), Color.blue);
    // }

    // private void PlaceRoomEntrance(GenerationContext context, int x, int y)
    // {
    //     MazeGenerator.AddDebugMarkToScheme(new Vector2Int(x, y), Color.yellow);
    // }
}
