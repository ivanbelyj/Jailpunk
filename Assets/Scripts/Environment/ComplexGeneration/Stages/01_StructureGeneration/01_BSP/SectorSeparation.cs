using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Base maze structure generation stage using BSP
/// </summary>
public class SectorSeparation : GenerationStage
{
    public override void RunStage()
    {
        var options = InitOptions(context);

        var generator = new BSPGenerator();

        var (bspNodes, connectivity) = generator.GenerateBSPNodes(options);

        GenerationData.SectorRects = GetSectorRects(bspNodes);
        GenerationData.RawCorridorsConnectivity = connectivity;

        // var corridorsAll = new List<CorridorSpace>();
        // foreach (BSPLeaf leaf in bspLeaves) {
        //     if (leaf.corridors != null) {
        //         foreach (CorridorSpace corridor in leaf.corridors) {
        //             corridorsAll.Add(corridor);
        //         }
        //     }
        // };
        // genContext.Corridors = corridorsAll;
    }

    private List<RectArea> GetSectorRects(IEnumerable<BSPNode> bspNodes) =>
        bspNodes
            .Select(node => node.rectArea)
            .Where(rectArea => rectArea != null)
            .ToList();

    private BSPGenerationOptions InitOptions(GenerationContext context) {
        var options = context.Settings.structureBSPOptions;
        var parameters = context.Request.Parameters;

        options.RootNodeSize = parameters.MazeSize;
        options.minLeafSize = parameters.MinSectorSize;
        options.maxLeafSize = parameters.MaxSectorSize;

        return options;
    }
}
