using UnityEngine;

public static class DebugColorMarkUtils
{
    private static Color[] colorsToMarkSectorZones = new[]
    {
        Color.red, Color.green, Color.blue, Color.yellow, 
        Color.magenta, Color.cyan, Color.white
    };
    private static int currentSectorColor = -1;

    public static Color GetNextColor() {
        currentSectorColor++;
        if (currentSectorColor >= colorsToMarkSectorZones.Length) {
            currentSectorColor = 0;
        }
        return colorsToMarkSectorZones[currentSectorColor];
    }
}
