using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DebugGUI : GenerationStage
{
    [SerializeField]
    private GUISkin guiSkin;
    private MazeScheme scheme;
    public override GenerationContext ProcessMaze(GenerationContext mazeData)
    {
        scheme = mazeData.MazeData.Scheme;
        return mazeData;
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(25, 25, 100, 30), "Regenerate")) 
        {
            FindObjectOfType<MazeGenerator>().CreateMaze();
        }
        if (scheme == null)
        {
            return;
        }

        StringBuilder msg = new StringBuilder();

        for (int y = 0; y < scheme.MapSize.y; y++)
        {
            for (int x = 0; x < scheme.MapSize.x; x++)  
            {
                SchemeTile tile = scheme.GetTileByPos(x, y);
                msg.Append(tile.TileType switch {
                    TileType.NoSpace => "  ",
                    TileType.Floor => "..",
                    TileType.LoadBearingWall => "==",
                    TileType.Wall => "--",
                    _ => "??"
                });
            }
            msg.Append("\n");
        }
        
        GUI.skin = guiSkin;
        GUI.Label(new Rect(20, 20, 1920, 1080), msg.ToString());
    }
}
