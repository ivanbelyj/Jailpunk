using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomBlocksGenerator
{
    private readonly GenerationSettings settings;

    public RoomBlocksGenerator(GenerationSettings settings)
    {
        this.settings = settings;
    }

    public List<BSPNode> GenerateBSPNodes(
        MazeScheme scheme,
        RectInt rect,
        BSPGenerationOptions roomBlockBSPOptions)
    {
        return GenerateBSPNodes(rect, roomBlockBSPOptions);
    }

    private List<BSPNode> GenerateBSPNodes(
        RectInt rect,
        BSPGenerationOptions roomBlockBSPOptions)
    {
        // Divide initial sector nodes (subsectors, roomblocks, "wide" corridors)
        var sectorPlanningOptions = GetSectorBSPGenerationOptions(rect, roomBlockBSPOptions);
        var sectorNodes = CreatePlanning(sectorPlanningOptions);

        // Segregate generated initial sector nodes which we can divide further
        var (subsectors, roomBlocks) = SegregateNotSplittedNodes(sectorNodes);

        // Divide subsector nodes (roomblocks, "narrow" corridors)
        var subsectorNodes = CreateSubsectors(subsectors.Select(x => x.rectArea.Rect));

        // Get subsector room blocks to divide them only
        var subsectorRoomBlocks = GetNotSplitted(subsectorNodes).ToList();

        // Divide room blocks to rooms
        var roomBlockNodes = CreateRooms(roomBlocks.Concat(subsectorRoomBlocks).Select(x => x.rectArea.Rect));

        return sectorNodes // To apply "wide" corridors"
            .Concat(subsectorNodes) // To apply "narrow" corridors
            .Concat(roomBlockNodes) // To apply rooms
            .ToList();
    }

    private List<BSPNode> CreateRooms(IEnumerable<RectInt> roomBlocks) {
        var result = new List<BSPNode>();
        foreach (var roomBlockRect in roomBlocks) {
            result.AddRange(CreatePlanning(GetRoomBSPOptions(roomBlockRect)));
        }
        return result;
    }

    private BSPGenerationOptions GetRoomBSPOptions(RectInt roomBlockRect) {
        var options = settings.roomBlockBSPOptions.Clone();
        options.RootNodeOffset = roomBlockRect.position;
        options.RootNodeSize = roomBlockRect.size;
        return options;
    }

    private List<BSPNode> CreateSubsectors(
        IEnumerable<RectInt> rects)
    {
        var nodes = new List<BSPNode>();
        foreach (var subsectorRect in rects) {
            var options = GetSectorBSPGenerationOptions(subsectorRect, settings.subsectorBSPOptions);
            nodes.AddRange(CreatePlanning(options));
        }
        return nodes;
    }

    private (List<BSPNode> subsectors, List<BSPNode> roomBlocks) SegregateNotSplittedNodes(
        IEnumerable<BSPNode> bspNodes)
    {
        var subsectors = new List<BSPNode>();
        var roomBlocks = new List<BSPNode>();
        foreach (var node in GetNotSplitted(bspNodes)) {
            (IsSubsector(node) ? subsectors : roomBlocks).Add(node);
        }
        return (subsectors, roomBlocks);
    }

    private IEnumerable<BSPNode> GetNotSplitted(IEnumerable<BSPNode> bspNodes)
        => bspNodes.Where(x => !x.IsSplit());

    /// <summary>
    /// It's supposed that bspLeaf is a splitted node
    /// </summary>
    private bool IsSubsector(BSPNode bspLeaf)
        => bspLeaf.AspectRatio <= settings.maxSubsectorRatio
            && Mathf.RoundToInt(Mathf.Sqrt(bspLeaf.rectArea.Rect.width * bspLeaf.rectArea.Rect.height))
                >= settings.subsectorMinSize;

    private List<BSPNode> CreatePlanning(
        BSPGenerationOptions options)
    {
        var bspGenerator = new BSPGenerator();
        var (nodes, graph) = bspGenerator.GenerateBSPNodes(options);
        return nodes;
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
