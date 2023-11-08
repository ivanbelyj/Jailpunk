using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Uses maze scheme to determine connectivity between sectors
/// </summary>
public class ConnectivityGeneration : GenerationStage
{
    public override GenerationContext ProcessMaze(GenerationContext context)
    {
        // Todo: traverse maze scheme and determine adjacent sectors
        // that could be connected
        return context;
    }
}
