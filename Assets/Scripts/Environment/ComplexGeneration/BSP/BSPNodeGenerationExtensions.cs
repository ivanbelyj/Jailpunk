using UnityEngine;

public static class BSPNodeGenerationExtensions
{
    /// <summary>
    /// Splits the node into two children.
    /// </summary>
    /// <returns>Did the nodes split</returns>
    public static bool GenerateChildNodes(
        this BSPNode node,
        BSPGenerationOptions options)
    {
        if (node.IsSplit())
        {
            return false; // The node is already split
        }

        bool isVerticalSplit = DetermineSplitOrientation(node, options);

        int splitSize = isVerticalSplit ? node.width : node.height;
        if (splitSize < 2 * options.minLeafSize + 2 * options.leafAreasOffset)
        {
            return false; // The node is too small
        }

        int splitPoint = GetSplitPoint(
            splitSize,
            options,
            options.splitMinimalLeaves ? (Random.value < 0.5f ? 0 : 1) : null);

        CreateChildNodes(node, isVerticalSplit, splitPoint, options);
        return true; // The split is completed
    }

    public static Graph<RectArea> GenerateAreas(
        this BSPNode node,
        BSPGenerationOptions options)
    {
        var areaConnectivity = new Graph<RectArea>();
        GenerateAreasRecursively(node, options, areaConnectivity);
        return areaConnectivity;
    }

    /// <returns>Is vertical split</returns>
    private static bool DetermineSplitOrientation(BSPNode node, BSPGenerationOptions options)
    {
        return node.AspectRatio >= options.minSplitRatio
            ? node.width > node.height
            : Random.Range(0, 1f) > 0.5f;
    }

    private static void CreateChildNodes(
        BSPNode node,
        bool isVerticalSplit,
        int splitPoint,
        BSPGenerationOptions options)
    {
        int leftOffset = Random.Range(0, options.leafAreasOffset + 1);
        int rightOffset = options.leafAreasOffset - leftOffset;

        node.leftChild = new BSPNode(
            node.x,
            node.y,
            isVerticalSplit ? splitPoint - leftOffset : node.width,
            isVerticalSplit ? node.height : splitPoint - leftOffset);
        node.rightChild = new BSPNode(
            isVerticalSplit ? node.x + splitPoint + rightOffset : node.x,
            isVerticalSplit ? node.y : node.y + splitPoint + rightOffset,
            isVerticalSplit ? node.width - splitPoint - rightOffset : node.width,
            isVerticalSplit ? node.height : node.height - splitPoint - rightOffset);

        node.intermediateArea = new RectArea(new RectInt(
            isVerticalSplit ? node.x + splitPoint - leftOffset : node.x,
            isVerticalSplit ? node.y : node.y + splitPoint - leftOffset,
            isVerticalSplit ? options.leafAreasOffset : node.width,
            isVerticalSplit ? node.height : options.leafAreasOffset));
    }

    private static void GenerateAreasRecursively(
        this BSPNode node,
        BSPGenerationOptions options,
        Graph<RectArea> areaConnectivity = null)
    {
        if (node.leftChild != null || node.rightChild != null)
        {
            node.leftChild?.GenerateAreasRecursively(options, areaConnectivity);
            node.rightChild?.GenerateAreasRecursively(options, areaConnectivity);

            ConnectAreas(node, areaConnectivity);
        }
        else
        {
            node.rectArea = node.CreateRandomArea(options);
        }
    }

    private static void ConnectAreas(BSPNode node, Graph<RectArea> areaConnectivity)
    {
        if (areaConnectivity != null && node.leftChild != null && node.rightChild != null)
        {
            var leftArea = node.leftChild.GetSomeArea();
            var rightArea = node.rightChild.GetSomeArea();

            if (leftArea != null && rightArea != null)
            {
                areaConnectivity.AddLink(leftArea, rightArea);
            }
        }
    }

    private static int GetSplitPoint(
        int splitSize,
        BSPGenerationOptions options,
        float? position)
    {
        return Mathf.RoundToInt(Mathf.Lerp(
            options.minLeafSize + options.leafAreasOffset,
            splitSize - options.minLeafSize - options.leafAreasOffset,
            position ?? Random.value));
    }

    private static int GenerateAreaSize(
        int leafSize,
        BSPGenerationOptions options)
    {
        int minSize = Mathf.Max(3, leafSize - options.maxRoomOffset);
        int maxSize = Mathf.Max(minSize, leafSize - options.minRoomOffset * 2);

        int areaSize = Random.Range(minSize, maxSize + 1);
        if (leafSize - areaSize == 1)
        {
            areaSize = leafSize - 2;
        }

        return areaSize;
    }

    private static RectArea CreateRandomArea(
        this BSPNode leaf,
        BSPGenerationOptions options)
    {
        int areaWidth = GenerateAreaSize(leaf.width, options);
        int areaHeight = GenerateAreaSize(leaf.height, options);

        Vector2Int areaPos = new Vector2Int(
            GenerateAreaPosition(leaf.width, areaWidth, options),
            GenerateAreaPosition(leaf.height, areaHeight, options));

        return new RectArea(new RectInt(
            leaf.x + areaPos.x,
            leaf.y + areaPos.y,
            areaWidth,
            areaHeight));
    }

    private static int GenerateAreaPosition(
        int leafSize,
        int areaSize,
        BSPGenerationOptions options)
    {
        return options.minRoomOffset / 2;
    }
}
