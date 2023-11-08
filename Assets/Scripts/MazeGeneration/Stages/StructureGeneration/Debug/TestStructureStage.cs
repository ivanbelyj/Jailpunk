using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CorridorsStructureGeneration))]
public class TestStructureStage : GenerationStage
{
    public List<RectSpacePair> testRooms;
    public override GenerationContext ProcessMaze(GenerationContext context)
    {
        var sectors = new List<RectArea>();
        testRooms.ForEach(pair => {
            sectors.Add(pair.rectSpace1);
            sectors.Add(pair.rectSpace2);
        });

        context.Sectors = sectors;

        var generator = new CorridorsGenerator();
        var corridors = new List<CorridorArea>();

        foreach (var pair in testRooms) {
            corridors.AddRange(generator
                .CreateCorridors(pair.rectSpace1, pair.rectSpace2,
                    GetComponent<CorridorsStructureGeneration>().corridorsBreadth));
        }
        context.Corridors = corridors;

        return context;
    }
}
