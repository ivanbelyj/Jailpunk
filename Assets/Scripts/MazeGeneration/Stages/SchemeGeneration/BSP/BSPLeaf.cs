using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based on https://habr.com/ru/articles/332832/
public class BSPLeaf
{
    public int y, x, width, height;

    public BSPLeaf leftChild;
    public BSPLeaf rightChild;
    public RectInt? room;
    public List<RectInt> halls;

    private BSPGenerationOptions options;

    public BSPLeaf(int X, int Y, int Width, int Height,
        BSPGenerationOptions options)
    {
       x = X;
       y = Y;
       width = Width;
       height = Height;

       this.options = options;
   }


    /// <summary>
    /// Splits the leaf into the two children
    /// </summary>
    /// <returns>Did the leaf split</returns>
    public bool CreateChildrenLeaves()
    {
        if (leftChild != null || rightChild != null)
            return false; // The leaf is already split

        // Is vertical split on the horizontal leaf
        bool isVerticalSplit = width > height;
        if (width > height && width / (float)height >=options.minSplitRatio)
            isVerticalSplit = true;
        else if (height > width && height / (float)width >= options.minSplitRatio)
            isVerticalSplit = false;
        // else 
        //     isVerticalSplit = Random.Range(0, 1f) > 0.5f;

        // The size of the side that will be split
        int splitSize = isVerticalSplit ? width : height;
        if (splitSize < 2 * options.minLeafSize)
            return false; // The leaf is too small

        int splitPoint = Random.Range(options.minLeafSize,
            splitSize - options.minLeafSize + 1);

        // Debug.Log("Split size: " + splitSize + "; split point: " + splitPoint);
        // if (splitSize < minLeafSize)
        //     Debug.LogWarning($"Split {splitSize} size exceeded minLeafSize");

        if (isVerticalSplit)
        {
            int rightChildWidth = width - splitPoint;
            // if (rightChildWidth < minLeafSize)
            //     return false;

            leftChild = new BSPLeaf(x, y, splitPoint, height, options);
            rightChild = new BSPLeaf(x + splitPoint, y, rightChildWidth, height, options);
        }
        else
        {
            int rightChildHeight = height - splitPoint;
            // if (rightChildHeight < minLeafSize)
            //     return false;

            leftChild = new BSPLeaf(x, y, width, splitPoint, options);
            rightChild = new BSPLeaf(x, y + splitPoint, width, rightChildHeight, options);
        }
        return true; // The split is completed
    }

    /// <summary>
    /// Generates all the rooms and coridors for this Leaf and all its children
    /// </summary>
    public void CreateRooms()
    {
        // If the leaf was split
        if (leftChild != null || rightChild != null)
        {
            leftChild?.CreateRooms();
            rightChild?.CreateRooms();

            if (leftChild != null && rightChild != null) {
                var leftRoom = leftChild.GetSomeRoom();
                var rightRoom = rightChild.GetSomeRoom();
                if (leftRoom != null && rightRoom != null) {
                    CreateHall(leftRoom.Value, rightRoom.Value);
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

    private RectInt CreateRandomRoom() {
        int roomWidth = GetRoomOffset(width);
        int roomHeight = GetRoomOffset(height);
        Vector2Int roomSize = new Vector2Int(roomWidth, roomHeight);
        // The room is in the leaf, but it's not right next to the side of the leaf
        // (else the rooms will merge)
        Vector2Int roomPos = new Vector2Int(
            Random.Range(0, width - roomSize.x),
            Random.Range(0, height - roomSize.y));
        return new RectInt(x + roomPos.x, y + roomPos.y,
            roomSize.x, roomSize.y);
    }

    /// <summary>
    /// Gets some room from the leaf or from its children
    /// </summary>
    public RectInt? GetSomeRoom()
    {
        if (room != null)
            return room.Value;
        else
        {
            RectInt? leftRoom = null;
            RectInt? rightRoom = null;
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


    private Vector2Int GetRandomPointInRoom(RectInt room) {
        Vector2Int point = new Vector2Int(
            Random.Range(room.xMin + 1, room.xMax - 2),
            Random.Range(room.yMin + 1, room.yMax - 2));
        return point;
    }

    /// <summary>
    /// Connects two rooms with halls.
    /// </summary>
    /// <param name="leftRoom"></param>
    /// <param name="rightRoom"></param>
    private void CreateHall(RectInt leftRoom, RectInt rightRoom) {
        halls = new List<RectInt>();

        Vector2Int pointLeft = GetRandomPointInRoom(leftRoom);
        Vector2Int pointRight = GetRandomPointInRoom(rightRoom);

        int w = pointRight.x - pointLeft.x;
        int h = pointRight.y - pointLeft.y;

        if (w < 0) {
            if (h < 0) {
                if (Random.value < 0.5f) {
                    halls.Add(new RectInt(pointRight.x, pointLeft.y, Mathf.Abs(w), 1));
                    halls.Add(new RectInt(pointRight.x, pointRight.y, 1, Mathf.Abs(h)));
                } else {
                    halls.Add(new RectInt(pointRight.x, pointRight.y, Mathf.Abs(w), 1));
                    halls.Add(new RectInt(pointLeft.x, pointRight.y, 1, Mathf.Abs(h)));
                }
            } else if (h > 0) {
                if (Random.value < 0.5f) {
                    halls.Add(new RectInt(pointRight.x, pointLeft.y, Mathf.Abs(w), 1));
                    halls.Add(new RectInt(pointRight.x, pointLeft.y, 1, Mathf.Abs(h)));
                } else {
                    halls.Add(new RectInt(pointRight.x, pointRight.y, Mathf.Abs(w), 1));
                    halls.Add(new RectInt(pointLeft.x, pointLeft.y, 1, Mathf.Abs(h)));
                }
            } else  // если (h == 0)
            {
                halls.Add(new RectInt(pointRight.x, pointRight.y, Mathf.Abs(w), 1));
            }
        } else if (w > 0) {
            if (h < 0) {
                if (Random.value < 0.5f) {
                    halls.Add(new RectInt(pointLeft.x, pointRight.y, Mathf.Abs(w), 1));
                    halls.Add(new RectInt(pointLeft.x, pointRight.y, 1, Mathf.Abs(h)));
                } else {
                    halls.Add(new RectInt(pointLeft.x, pointLeft.y, Mathf.Abs(w), 1));
                    halls.Add(new RectInt(pointRight.x, pointRight.y, 1, Mathf.Abs(h)));
                }
            } else if (h > 0) {
                if (Random.value < 0.5f) {
                    halls.Add(new RectInt(pointLeft.x, pointLeft.y, Mathf.Abs(w), 1));
                    halls.Add(new RectInt(pointRight.x, pointLeft.y, 1, Mathf.Abs(h)));
                } else {
                    halls.Add(new RectInt(pointLeft.x, pointRight.y, Mathf.Abs(w), 1));
                    halls.Add(new RectInt(pointLeft.x, pointLeft.y, 1, Mathf.Abs(h)));
                }
            } else  // если (h == 0)
            {
                halls.Add(new RectInt(pointLeft.x, pointLeft.y, Mathf.Abs(w), 1));
            }
        } else  // если (w == 0)
        {
            if (h < 0) {
                halls.Add(new RectInt(pointRight.x, pointRight.y, 1, Mathf.Abs(h)));
            } else if (h > 0) {
                halls.Add(new RectInt(pointLeft.x, pointLeft.y, 1, Mathf.Abs(h)));
            }
        }
    }


    


}
