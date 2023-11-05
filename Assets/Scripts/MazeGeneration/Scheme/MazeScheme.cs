using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents original design of the maze
/// </summary>
public class MazeScheme : MonoBehaviour
{
    private SchemeTile[,] Map { get; set; }

    public MazeScheme(Vector2Int mapSize) {
        Map = new SchemeTile[mapSize.y, mapSize.x];
        for (int row = 0; row < mapSize.y; row++) {
            for (int col = 0; col < mapSize.x; col++) {
                Map[row, col] = new SchemeTile() {
                    SectorId = null,
                    TileType = TileType.NoSpace
                };
            }
        }
    }

    public SchemeTile GetTileByPos(int x, int y) => Map[y, x];
}
