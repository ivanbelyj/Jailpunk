using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Applies previous generated BSP data
/// </summary>
public class ApplyBSP : GenerationStage
{
    public override GenerationContext ProcessMaze(GenerationContext context)
    {
        var walls = context.MazeData.Walls =
            new int[generationData.MazeSize.y, generationData.MazeSize.x];
        
        foreach (BSPLeaf leaf in context.BSPLeaves) {
            ApplyLeaf(walls, leaf);
        }

        return context;
    }

    private void ApplyLeaf(int[,] walls, BSPLeaf leaf) {
        if (leaf.room != null) {
            RectInt room = leaf.room.Value;
            ApplyRect(walls, room);
        }

        if (leaf.halls != null) {
            foreach (RectInt hall in leaf.halls) {
                Debug.Log("Hall: " + hall); 
                ApplyRect(walls, hall);
            }
        }
    }

    private void ApplyRect(int[,] walls, RectInt rect) {
        int roomRightLimit = rect.x + rect.width - 1;
        int roomBottomLimit = rect.y + rect.height - 1;
        for (int row = rect.y; row <= roomBottomLimit; row++) {
            if (row == rect.y || row == roomBottomLimit) {
                for (int col = rect.x; col <= roomRightLimit; col++) {
                    walls[row, col] = 1;
                }
            } else {
                walls[row, rect.x] = 1;
                walls[row, roomRightLimit] = 1;
            }
        }
    }
}
