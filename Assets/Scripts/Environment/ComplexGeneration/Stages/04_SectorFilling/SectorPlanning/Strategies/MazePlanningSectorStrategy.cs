// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;

// public class MazePlanningSectorStrategy : ISectorPlanningStrategy
// {
//     private readonly MazeWithRoomsGenerator mazeGenerator;

//     public MazePlanningSectorStrategy()
//     {
//         mazeGenerator = new MazeWithRoomsGenerator();
//     }

//     public List<SchemeArea> PlanSector(GeneratedSectorInfo sector, GenerationContext context)
//     {
//         GenerateMaze(sector, context);
//         return new();
//     }

//     private void GenerateMaze(GeneratedSectorInfo generatedSector, GenerationContext context)
//     {
//         var area = generatedSector.RectArea;
//         if (area == null) return;

//         mazeGenerator.Generate(area.Rect, (x, y, cellType) =>
//         {
//             ApplyCell(context, x, y, cellType);
//         });
//     }

//     private void ApplyCell(GenerationContext context, int x, int y, CellType cellType)
//     {
//         switch (cellType)
//         {
//             case CellType.Wall:
//                 PlaceWall(context, x, y);
//                 break;
//             case CellType.Path:
//                 PlacePath(context, x, y);
//                 break;
//             case CellType.Room:
//                 PlaceRoom(context, x, y);
//                 break;
//             case CellType.RoomEntrance:
//                 PlaceRoomEntrance(context, x, y);
//                 break;
//         }
//     }

//     private void PlaceWall(GenerationContext context, int x, int y)
//     {
//         MazeGenerator.AddDebugMarkToScheme(new Vector2Int(x, y), Color.black);
//     }

//     private void PlacePath(GenerationContext context, int x, int y)
//     {
//         MazeGenerator.AddDebugMarkToScheme(new Vector2Int(x, y), Color.white);
//     }

//     private void PlaceRoom(GenerationContext context, int x, int y)
//     {
//         MazeGenerator.AddDebugMarkToScheme(new Vector2Int(x, y), Color.gray);
//     }

//     private void PlaceRoomEntrance(GenerationContext context, int x, int y)
//     {
//         MazeGenerator.AddDebugMarkToScheme(new Vector2Int(x, y), Color.yellow);
//     }
// }
