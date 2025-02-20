using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DebugGUI : GenerationStage
{
    [SerializeField]
    private GUISkin guiSkin;
    private ComplexScheme scheme;
    private int generationNumber = 0;

    public int offsetX;
    public int offsetY;
    
    public override void RunStage()
    {
        scheme = context.ComplexData.Scheme;
    }

    private ComplexGenerator complexGenerator;
    private ComplexGenerator ComplexGenerator {
        get {
            if (complexGenerator == null)
            {
                complexGenerator = FindAnyObjectByType<ComplexGenerator>();
            }
            return complexGenerator;
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 125, 250, 60), "Regenerate")) 
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
        ComplexGenerator.Regenerate($"ComplexGrid{++generationNumber}");
    }
}
