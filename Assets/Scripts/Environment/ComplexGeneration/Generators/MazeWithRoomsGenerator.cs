using System;
using UnityEngine;
using System.Collections.Generic;

// AI-generated code, so it can be buggy
public class MazeWithRoomsGenerator
{
    public enum CellType
    {
        Wall,
        Path,
        Room,
        RoomEntrance
    }

    private readonly System.Random random = new System.Random();

    public CellType[,] Generate(RectInt rect)
    {
        var grid = InitializeGrid(rect);
        GenerateComplex(grid);
        // PopulateRooms(grid);
        return grid;
    }

    public static void TraverseGrid(RectInt rect, CellType[,] grid, Action<int, int, CellType> cellCallback)
    {
        for (int y = 0; y < rect.height; y++)
        {
            for (int x = 0; x < rect.width; x++)
            {
                cellCallback(rect.x + x, rect.y + y, grid[x, y]);
            }
        }
    }

    private CellType[,] InitializeGrid(RectInt rect)
    {
        var grid = new CellType[rect.width, rect.height];
        FillGridWithWalls(grid);
        return grid;
    }

    private void FillGridWithWalls(CellType[,] grid)
    {
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                grid[x, y] = CellType.Wall;
            }
        }
    }

    private void GenerateComplex(CellType[,] grid)
    {
        var start = GetRandomStart(grid);
        var stack = new Stack<Vector2Int>();
        stack.Push(start);
        grid[start.x, start.y] = CellType.Path;

        while (stack.Count > 0)
        {
            var current = stack.Peek();
            var neighbors = GetUnvisitedNeighbors(grid, current);

            if (neighbors.Count > 0)
            {
                var next = neighbors[random.Next(neighbors.Count)];
                RemoveWallBetween(grid, current, next);
                grid[next.x, next.y] = CellType.Path;
                stack.Push(next);
            }
            else
            {
                stack.Pop();
            }
        }
    }

    private Vector2Int GetRandomStart(CellType[,] grid)
    {
        int x = random.Next(1, grid.GetLength(0) / 2) * 2 - 1;
        int y = random.Next(1, grid.GetLength(1) / 2) * 2 - 1;
        return new Vector2Int(x, y);
    }

    private List<Vector2Int> GetUnvisitedNeighbors(CellType[,] grid, Vector2Int current)
    {
        var neighbors = new List<Vector2Int>();
        var directions = new[]
        {
            new Vector2Int(0, 2), new Vector2Int(0, -2),
            new Vector2Int(2, 0), new Vector2Int(-2, 0)
        };

        foreach (var dir in directions)
        {
            var neighbor = current + dir;
            if (IsWithinBounds(grid, neighbor) && grid[neighbor.x, neighbor.y] == CellType.Wall)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    private bool IsWithinBounds(CellType[,] grid, Vector2Int position)
    {
        return position.x >= 0 && position.x < grid.GetLength(0)
               && position.y >= 0 && position.y < grid.GetLength(1);
    }

    private void RemoveWallBetween(CellType[,] grid, Vector2Int current, Vector2Int next)
    {
        var wall = (current + next) / 2;
        grid[wall.x, wall.y] = CellType.Path;
    }

    private void PopulateRooms(CellType[,] grid)
    {
        int roomCount = random.Next(3, 6);

        for (int i = 0; i < roomCount; i++)
        {
            var roomRect = GenerateRoomRect(grid);
            if (roomRect != null) {
                CarveRoom(grid, roomRect.Value);
                AddRoomEntrance(grid, roomRect.Value);
            }
        }
    }

    private RectInt? GenerateRoomRect(CellType[,] grid)
    {
        int width = random.Next(3, 6);
        int height = random.Next(3, 6);

        int x = random.Next(1, (grid.GetLength(0) - width) / 2) * 2;
        int y = random.Next(1, (grid.GetLength(1) - height) / 2) * 2;

        var rect = new RectInt(x, y, width, height);

        if (CanPlaceRoom(grid, rect))
        {
            return rect;
        }

        return null;
    }

    private bool CanPlaceRoom(CellType[,] grid, RectInt rect)
    {
        for (int y = rect.yMin; y < rect.yMax; y++)
        {
            for (int x = rect.xMin; x < rect.xMax; x++)
            {
                if (grid[x, y] != CellType.Wall)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void CarveRoom(CellType[,] grid, RectInt rect)
    {
        for (int y = rect.yMin; y < rect.yMax; y++)
        {
            for (int x = rect.xMin; x < rect.xMax; x++)
            {
                grid[x, y] = CellType.Room;
            }
        }
    }

    private void AddRoomEntrance(CellType[,] grid, RectInt room)
    {
        var possibleEntrances = GetRoomEdges(room);

        if (possibleEntrances.Count > 0)
        {
            var entrance = possibleEntrances[random.Next(possibleEntrances.Count)];
            grid[entrance.x, entrance.y] = CellType.RoomEntrance;
        }
    }

    private List<Vector2Int> GetRoomEdges(RectInt room)
    {
        var edges = new List<Vector2Int>();

        for (int x = room.xMin; x < room.xMax; x++)
        {
            edges.Add(new Vector2Int(x, room.yMin - 1));
            edges.Add(new Vector2Int(x, room.yMax));
        }

        for (int y = room.yMin; y < room.yMax; y++)
        {
            edges.Add(new Vector2Int(room.xMin - 1, y));
            edges.Add(new Vector2Int(room.xMax, y));
        }

        return edges;
    }
}

