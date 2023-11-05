using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DebugGUI : GenerationStage
{
    [SerializeField]
    private GUISkin guiSkin;
    private int[,] walls;
    public override GenerationContext ProcessMaze(GenerationContext mazeData)
    {
        walls = mazeData.MazeData.Walls;
        return mazeData;
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(25, 25, 100, 30), "Regenerate")) 
        {
            FindObjectOfType<MazeGenerator>().CreateMaze();
        }
        if (walls == null)
        {
            return;
        }

        int[,] maze = walls;
        int rowMax = maze.GetUpperBound(0);
        int colMax = maze.GetUpperBound(1);

        StringBuilder msg = new StringBuilder();

        for (int row = 0; row <= rowMax; row++)
        {
            for (int col = 0; col <= colMax; col++)
            {
                if (maze[row, col] == 0)
                {
                    msg.Append("..");
                }
                else
                {
                    msg.Append("==");
                }
            }
            msg.Append("\n");
        }

        // GUIStyle style = new GUIStyle() {
        //     fontSize = fontSize,
        //     // font = Resources.Load<Font>("AnonymousPro-Regular.ttf")
        // };
        // style.normal.textColor = Color.white;
        GUI.skin = guiSkin;
        GUI.Label(new Rect(20, 20, 1920, 1080), msg.ToString());
    }
}
