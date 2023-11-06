using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based on https://habr.com/ru/articles/332832/
/// <summary>
/// Binary space partition leaf including some generation logic
/// </summary>
public class BSPLeaf
{
    public int y, x, width, height;

    public BSPLeaf leftChild;
    public BSPLeaf rightChild;
    public RectSpace room;

    /// <summary>
    /// Corridors connecting some rooms from left and right children
    /// </summary>
    public List<CorridorSpace> corridors;

    private BSPGenerationOptions options;

    public BSPLeaf(int x, int y, int width, int height,
        BSPGenerationOptions options)
    {
       this.x = x;
       this.y = y;
       this.width = width;
       this.height = height;

       this.options = options;
   }


    /// <summary>
    /// Splits the leaf into the two children
    /// </summary>
    /// <returns>Did the leaf split</returns>
    public bool GenerateChildrenLeaves()
    {
        if (leftChild != null || rightChild != null)
            return false; // The leaf is already split

        // Is vertical split on the horizontal leaf
        bool isVerticalSplit;  // = width > height;
        if (width > height && width / (float)height >= options.minSplitRatio)
            isVerticalSplit = true;
        else if (height > width && height / (float)width >= options.minSplitRatio)
            isVerticalSplit = false;
        else 
            isVerticalSplit = Random.Range(0, 1f) > 0.5f;

        // The size of the side that will be split
        int splitSize = isVerticalSplit ? width : height;
        if (splitSize < 2 * options.minLeafSize)
            return false; // The leaf is too small

        int splitPoint = Random.Range(options.minLeafSize,
            splitSize - options.minLeafSize + 1);

        if (isVerticalSplit)
        {
            int rightChildWidth = width - splitPoint;

            leftChild = new BSPLeaf(x, y, splitPoint, height, options);
            rightChild = new BSPLeaf(x + splitPoint, y, rightChildWidth, height, options);
        }
        else
        {
            int rightChildHeight = height - splitPoint;

            leftChild = new BSPLeaf(x, y, width, splitPoint, options);
            rightChild = new BSPLeaf(x, y + splitPoint, width, rightChildHeight, options);
        }
        return true; // The split is completed
    }

    /// <summary>
    /// Generates all the rooms for this Leaf and all its children
    /// </summary>
    public void GenerateRoomsAndCorridors()
    {
        var corridorsGenerator = new CorridorsGenerator();
        // If the leaf was split
        if (leftChild != null || rightChild != null)
        {
            leftChild?.GenerateRoomsAndCorridors();
            rightChild?.GenerateRoomsAndCorridors();

            if (leftChild != null && rightChild != null) {
                var leftRoom = leftChild.GetSomeRoom();
                var rightRoom = rightChild.GetSomeRoom();
                if (leftRoom != null && rightRoom != null) {
                    corridors =
                        corridorsGenerator.CreateCorridors(leftRoom, rightRoom);
                }
            }
        }
        else
        {
            // The leaf is ready for creating a room
            room = CreateRandomRoom();
        }
    }

    private int GetRoomOffset(int leafSize) {
        int min = leafSize - options.maxRoomInnerOffset;
        if (min < 3)
            min = 3;
        int max = leafSize - options.maxRoomOuterOffset;
        if (max < min)
            max = min;
        return Random.Range(min, max + 1);
    }

    private RectSpace CreateRandomRoom() {
        int roomWidth = GetRoomOffset(width);
        int roomHeight = GetRoomOffset(height);
        Vector2Int roomSize = new Vector2Int(roomWidth, roomHeight);
        // The room is in the leaf, but it's not right next to the side of the leaf
        // (else the rooms will merge)
        Vector2Int roomPos = new Vector2Int(
            Random.Range(0, width - roomSize.x),
            Random.Range(0, height - roomSize.y));
        return new RectSpace(new RectInt(x + roomPos.x, y + roomPos.y,
            roomSize.x, roomSize.y));
    }

    /// <summary>
    /// Gets some room from the leaf or from its children
    /// </summary>
    public RectSpace GetSomeRoom()
    {
        if (room != null)
            return room;
        else
        {
            RectSpace leftRoom = null;
            RectSpace rightRoom = null;
            if (leftChild != null)
            {
                leftRoom = leftChild.GetSomeRoom();
            }
            if (rightChild != null)
            {
                rightRoom = rightChild.GetSomeRoom();
            }
            if (leftRoom == null && rightRoom == null)
                return null;
            else if (rightRoom == null)
                return leftRoom;
            else if (leftRoom == null)
                return rightRoom;
            else if (Random.value > 0.5f)
                return leftRoom;
            else
                return rightRoom;
        }
    }
}
