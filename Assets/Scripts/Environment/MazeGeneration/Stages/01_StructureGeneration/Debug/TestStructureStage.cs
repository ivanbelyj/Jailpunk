using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class RectSpacePair
{
    public bool useInGeneration = true;
    public RectArea rectSpace1;
    public RectArea rectSpace2;
}

[RequireComponent(typeof(CorridorsStructureGeneration))]
public class TestStructureStage : GenerationStage
{
    [Header("Debug")]
    public List<RectSpacePair> testRooms;
    public override GenerationContext ProcessMaze(GenerationContext context)
    {
        var sectors = new List<RectArea>();
        testRooms.ForEach(pair => {
            if (!pair.useInGeneration)
                return;
            sectors.Add(pair.rectSpace1);
            sectors.Add(pair.rectSpace2);
        });

        context.SectorRects = sectors;

        var generator = new CorridorsGenerator(true);
        var corridors = new List<CorridorArea>();

        foreach (var pair in testRooms) {
            if (!pair.useInGeneration)
                continue;
            var generatedCorridors = generator
                .CreateCorridors(pair.rectSpace1, pair.rectSpace2,
                    GetComponent<CorridorsStructureGeneration>()
                        .corridorsBreadth);
            if (generatedCorridors == null) {
                Debug.LogWarning("Cannot connect test rooms");
                continue;
            }
            corridors.AddRange(generatedCorridors);
        }
        context.Corridors = corridors;

        return context;
    }
}
