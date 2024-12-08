using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DebugGUI : GenerationStage
{
    [SerializeField]
    private GUISkin guiSkin;
    private MazeScheme scheme;
    private int generationNumber = 0;
    
    public override GenerationContext ProcessMaze(GenerationContext mazeData)
    {
        scheme = mazeData.MazeData.Scheme;
        return mazeData;
    }

    // For debug
    private static MazeGenerator mazeGenerator;
    private static MazeGenerator MazeGenerator {
        get {
            if (mazeGenerator == null)
                mazeGenerator = FindObjectOfType<MazeGenerator>();
            return mazeGenerator;
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(25, 25, 200, 60), "Regenerate")) 
        {
            Regenerate();
        }
        if (scheme == null)
        {
            return;
        }

        GUI.skin = guiSkin;
        GUI.Label(new Rect(20, 20, 1920, 1080), scheme.ToString());
    }

    private void Regenerate() {
        MazeGenerator.Regenerate($"MazeGrid{++generationNumber}");
    }
}
