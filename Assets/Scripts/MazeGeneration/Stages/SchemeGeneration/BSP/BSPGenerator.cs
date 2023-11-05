using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates random BSP structure and returns all the leaves
/// </summary>
public class BSPGenerator
{
    public List<BSPLeaf> GenerateBSPLeaves(
        BSPGenerationOptions options
        // Vector2Int rootLeafSize,
        // int minLeafSize,
        // int maxLeafSize,
        // float splitLeafProbability = 0.75f,
        // float minSplitRatio = 1.25f
        ) {
        List<BSPLeaf> leavesToSplit = new List<BSPLeaf>();

        // Create the root leaf
        BSPLeaf root = new BSPLeaf(0, 0, options.rootLeafSize.x, options.rootLeafSize.y,
            options);
        leavesToSplit.Add(root);

        // bool isSplit = true;
        
        // Until there's no leaves that could be split
        // while (isSplit)
        // {
            // isSplit = false;
            for (int i = 0; i < leavesToSplit.Count; i++)
            {
                BSPLeaf l = leavesToSplit[i];
                // If the list is not split yet
                if (l.leftChild == null && l.rightChild == null)
                {
                    if (l.width > options.maxLeafSize
                        || l.height > options.maxLeafSize
                        || Random.value <= options.splitLeafProbability) {
                        if (l.CreateChildrenLeaves())
                        {
                            leavesToSplit.Add(l.leftChild);
                            leavesToSplit.Add(l.rightChild);
                            // isSplit = true;
                        }
                    }
                }
            }
        // }

        root.CreateRooms();
        return leavesToSplit;
    }
}
