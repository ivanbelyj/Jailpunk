using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates random BSP structure and returns all the nodes
/// </summary>
public class BSPGenerator
{
    private const int SplitIterationsLimit = 10000;

    // To support consistent split probabilities using multiple iterations
    private readonly Dictionary<BSPNode, float> RandomValuesByNode = new();

    /// <summary>
    /// Generates random BSP nodes and their connectivity
    /// </summary>
    public (List<BSPNode>, Graph<RectArea>) GenerateBSPNodes(
        BSPGenerationOptions options)
    {
        var rootNode = new BSPNode(
            options.RootNodeOffset.x,
            options.RootNodeOffset.y,
            options.RootNodeSize.x,
            options.RootNodeSize.y);

        var nodes = SplitNodes(rootNode, options);

        return (nodes, rootNode.GenerateAreas(options));
    }

    private void ValidateOptions(BSPGenerationOptions options) {
        if (options.minLeafSize < 1
            || options.maxLeafSize < 1
            || options.RootNodeSize.x < 1
            || options.RootNodeSize.y < 1) {
            Debug.LogError($"Invalid {nameof(BSPGenerationOptions)}.");
        }
    }

    private List<BSPNode> SplitNodes(BSPNode initialNode, BSPGenerationOptions options) {
        var nodes = new List<BSPNode>() { initialNode };
        var newNodesAdded = true;
        int interationCount = 0;
        while (newNodesAdded) {
            interationCount++;

            if (interationCount > SplitIterationsLimit) {
                Debug.LogError(
                    "Error on BSP generation: split iterations limit exceeded");
                break;
            }

            newNodesAdded = false;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (SplitAndAddChildNodes(nodes[i], nodes, options)) {
                    newNodesAdded = true;
                }
            }
        }
        
        return nodes;
    }

    /// <returns>True if new nodes were added</returns>
    private bool SplitAndAddChildNodes(
        BSPNode nodeToSplit,
        List<BSPNode> nodesToSplit,
        BSPGenerationOptions options)
    {
        var newNodesAdded = false;
        if (!nodeToSplit.IsSplit())
        {
            if (ShouldSplit(nodeToSplit, options)) {
                if (nodeToSplit.GenerateChildNodes(options))
                {
                    nodesToSplit.Add(nodeToSplit.leftChild);
                    nodesToSplit.Add(nodeToSplit.rightChild);
                    newNodesAdded = true;
                }
            }
        }
        return newNodesAdded;
    }

    private bool ShouldSplit(BSPNode nodeToSplit, BSPGenerationOptions options) {
        if (!RandomValuesByNode.ContainsKey(nodeToSplit)) {
            RandomValuesByNode.Add(nodeToSplit, Random.value);
        }
        return nodeToSplit.width > options.maxLeafSize
            || nodeToSplit.height > options.maxLeafSize
            || RandomValuesByNode[nodeToSplit] <= options.splitLeafProbability
                && (!options.useMaxSplitRatio
                || nodeToSplit.AspectRatio <= options.maxSplitRatio);
    }
}
