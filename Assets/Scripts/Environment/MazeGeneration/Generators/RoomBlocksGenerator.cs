using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// [System.Serializable]
// public class RoomBlocksPlanningOptions
// {
//     [Tooltip("Minimum room size including tiles for walls")]
//     [Min(3)]
//     public int minRoomBlockSize = 12;

//     [Tooltip(
//         "Maximum room size including tiles for walls. It will not be taken " +
//         "into account if it exceeds the size of the generated sector")]
//     [Min(3)]
//     public int maxRoomBlockSize = 56;

//     public int corridorWidth = 2;
// }

public class RoomBlocksGenerator
{
    private readonly GenerationSettings settings;

    public RoomBlocksGenerator(GenerationSettings settings)
    {
        this.settings = settings;
    }

    public void GenerateAndApply(
        MazeScheme scheme,
        RectInt rect,
        BSPGenerationOptions roomBlockBSPOptions)
    {
        var sectorPlanningOptions = GetSectorBSPGenerationOptions(rect, roomBlockBSPOptions);
        var sectorNodes = ApplyPlanning(scheme, sectorPlanningOptions);

        var (subsectors, roomBlocks) = SegregateNodes(sectorNodes);

        var subsectorNodes = ApplySubsectors(scheme, subsectors.Select(x => x.rectArea.Rect));
        var subsectorRoomBlocks = GetLeaves(subsectorNodes).ToList();

        ApplyRooms(
            scheme,
            roomBlocks.Concat(subsectorRoomBlocks).Select(x => x.rectArea.Rect));
    }

    private void ApplyRooms(MazeScheme scheme, IEnumerable<RectInt> roomBlocks) {
        foreach (var roomBlockRect in roomBlocks) {
            ApplyPlanning(scheme, GetRoomBSPOptions(roomBlockRect));
        }
    }

    private BSPGenerationOptions GetRoomBSPOptions(RectInt roomBlockRect) {
        var options = settings.roomBlockBSPOptions.Clone();
        options.RootNodeOffset = roomBlockRect.position;
        options.RootNodeSize = roomBlockRect.size;
        return options;
    }

    private List<BSPNode> ApplySubsectors(
        MazeScheme scheme,
        IEnumerable<RectInt> rects)
    {
        var nodes = new List<BSPNode>();
        foreach (var subsectorRect in rects) {
            var options = GetSectorBSPGenerationOptions(subsectorRect, settings.subsectorBSPOptions);
            nodes.AddRange(ApplyPlanning(scheme, options));
        }
        return nodes;
    }

    private (List<BSPNode> subsectors, List<BSPNode> roomBlocks) SegregateNodes(
        IEnumerable<BSPNode> bspNodes)
    {
        var subsectors = new List<BSPNode>();
        var roomBlocks = new List<BSPNode>();
        foreach (var node in GetLeaves(bspNodes)) {
            (IsSubsector(node) ? subsectors : roomBlocks).Add(node);
        }
        return (subsectors, roomBlocks);
    }

    private IEnumerable<BSPNode> GetLeaves(IEnumerable<BSPNode> bspNodes)
        => bspNodes.Where(x => !x.IsSplit());

    /// <summary>
    /// It's supposed that bspLeaf is a splitted node
    /// </summary>
    private bool IsSubsector(BSPNode bspLeaf)
        => bspLeaf.AspectRatio <= settings.maxSubsectorRatio
            && Mathf.RoundToInt(Mathf.Sqrt(bspLeaf.rectArea.Rect.width * bspLeaf.rectArea.Rect.height))
                >= settings.subsectorMinSize;

    private List<BSPNode> ApplyPlanning(MazeScheme scheme, BSPGenerationOptions options) {
        var bspGenerator = new BSPGenerator();
        var (nodes, graph) = bspGenerator.GenerateBSPNodes(options);
        ApplyNodeWalls(scheme, nodes);
        return nodes;
    }

    private void ApplyNodeWalls(
        MazeScheme scheme,
        List<BSPNode> nodes)
    {
        foreach (var node in nodes) {
            if (node.rectArea != null) {
                ApplyRectWalls(scheme, node.rectArea);
            }
        }
    }

    private void ApplyRectWalls(MazeScheme scheme, RectArea rect) {
        StructureUtils.TraverseRect(rect.Rect, (x, y, isBorder) => {
            SchemeTile tile = scheme.GetTileByPos(x, y);
            if (isBorder && tile.TileType != TileType.LoadBearingWall) {
                tile.TileType = TileType.Wall;
            }
        });
    }

    private BSPGenerationOptions GetSectorBSPGenerationOptions(
        RectInt rect,
        BSPGenerationOptions sectorBSPOptions)
    {
        var options = sectorBSPOptions.Clone();
        var (size, minLeafSize, maxLeafSize) = GetParams(rect, options);
        options.RootNodeSize = size;
        options.RootNodeOffset = rect.position;
        options.minLeafSize = minLeafSize;
        options.maxLeafSize = maxLeafSize;

        return options;

        // return new BSPGenerationOptions() {
        //     rootNodeSize = size,
        //     rootNodeOffset = rect.position,
        //     minLeafSize = minLeafSize,
        //     maxLeafSize = maxLeafSize,
        //     leafAreasOffset = options.corridorWidth,
        //     splitLeafProbability = 1f,
        //     useMaxSplitRatio = true,
        //     maxSplitRatio = 1.5f,
        //     splitMinimalLeaves = true
        // };
    }

    private (Vector2Int Size, int MinLeafSize, int MaxLeafSize) GetParams(
        RectInt rect,
        BSPGenerationOptions options)
    {
        var size = rect.size;
        var minSectorSize = Mathf.Min(size.x, size.y);
        var minSize = Mathf.Min(minSectorSize, options.minLeafSize);
        var maxSize = Mathf.Min(minSectorSize, options.maxLeafSize);
        return (size, minSize, maxSize);
    }
}
