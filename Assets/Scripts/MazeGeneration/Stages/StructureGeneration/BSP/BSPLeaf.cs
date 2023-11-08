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
    public RectArea room;

    /// <summary>
    /// Corridors connecting some rooms from left and right children
    /// </summary>
    // public List<CorridorSpace> corridors;

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

        // size + 1 because the new leaves should use
        // the same wall in the future
        if (isVerticalSplit)
        {
            int rightChildWidth = width - splitPoint;

            leftChild = new BSPLeaf(x, y, splitPoint + 1, height, options);
            rightChild = new BSPLeaf(x + splitPoint, y, rightChildWidth,
                height, options);
        }
        else
        {
            int rightChildHeight = height - splitPoint;

            leftChild = new BSPLeaf(x, y, width, splitPoint + 1, options);
            rightChild = new BSPLeaf(x, y + splitPoint, width,
                rightChildHeight, options);
        }
        return true; // The split is completed
    }

    /// <summary>
    /// Generates all the rooms for this Leaf and all its children
    /// and sets rooms connectivity into a graph is it's passed
    /// </summary>
    public void GenerateRoomsAndCorridors(Graph<RectArea> roomsConnectivity = null)
    {
        var corridorsGenerator = new CorridorsGenerator();
        // If the leaf was split
        if (leftChild != null || rightChild != null)
        {
            leftChild?.GenerateRoomsAndCorridors(roomsConnectivity);
            rightChild?.GenerateRoomsAndCorridors(roomsConnectivity);

            if (roomsConnectivity != null && leftChild != null
                && rightChild != null) {
                var leftRoom = leftChild.GetSomeRoom();
                var rightRoom = rightChild.GetSomeRoom();
                if (leftRoom != null && rightRoom != null) {
                    roomsConnectivity.AddLink(leftRoom, rightRoom);
                    // corridors =
                    //     corridorsGenerator.CreateCorridors(leftRoom, rightRoom);
                }
            }
        }
        else
        {
            // The leaf is ready for creating a room
            room = CreateRandomRoom();
        }
    }

    private int GenerateRoomSize(int leafSize) {
        int minSize = leafSize - options.maxRoomOffset;
        if (minSize < 3)
            minSize = 3;
        int maxSize = leafSize - options.minRoomOffset * 2;
        if (maxSize < minSize)
            maxSize = minSize;
        int roomSize = Random.Range(minSize, maxSize + 1);
        
        int offset = leafSize - roomSize;
        // Offset should not be 1 to avoid double walls
        if (offset == 1) {
            roomSize = leafSize - 2;
        }

        return roomSize;
    }

    private RectArea CreateRandomRoom() {
        int roomWidth = GenerateRoomSize(width);
        int roomHeight = GenerateRoomSize(height);
        // Vector2Int roomSize = new Vector2Int(roomWidth, roomHeight);

        int GenerateRoomPos(int leafSize, int roomSize) {
            // int res = Random.Range(options.minRoomOffset / 2,
            //     leafSize - roomSize);
            
            int res = options.minRoomOffset / 2;
            // if (options.maxRoomOffset > options.minRoomOffset) {
            //     res += Random.Range(0, (options.maxRoomOffset
            //         - options.minRoomOffset) / 2);
            // }
            // To avoid double walls
            // if (res == 1)
            //     res = 3;
                // res = Random.value < 0.5f || roomSize == leafSize ? 0 : 2;
            return res;
        }

        Vector2Int roomPos = new Vector2Int(
            // Random.Range(0, width - roomSize.x),
            // Random.Range(0, height - roomSize.y)
            GenerateRoomPos(width, roomWidth),
            GenerateRoomPos(height, roomHeight)
            );
        
        return new RectArea(new RectInt(x + roomPos.x, y + roomPos.y,
            roomWidth, roomHeight));
    }

    /// <summary>
    /// Gets some room from the leaf or from its children
    /// </summary>
    public RectArea GetSomeRoom()
    {
        if (room != null)
            return room;
        else
        {
            RectArea leftRoom = null;
            RectArea rightRoom = null;
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
