// using UnityEngine;

// public static class BSPLeafExtensions
// {
//     /// <summary>
//     /// Splits the leaf into the two children
//     /// </summary>
//     /// <returns>Did the leaf split</returns>
//     public static bool GenerateChildrenLeaves(
//         this BSPLeaf leaf,
//         BSPGenerationOptions options)
//     {
//         if (leaf.IsSplit())
//             return false; // The leaf is already split

//         // Is vertical split on the horizontal leaf
//         bool isVerticalSplit;  // = width > height;
//         if (leaf.width > leaf.height && leaf.width / (float)leaf.height >= options.minSplitRatio)
//             isVerticalSplit = true;
//         else if (leaf.height > leaf.width && leaf.height / (float)leaf.width >= options.minSplitRatio)
//             isVerticalSplit = false;
//         else 
//             isVerticalSplit = Random.Range(0, 1f) > 0.5f;

//         // The size of the side that will be split
//         int splitSize = isVerticalSplit ? leaf.width : leaf.height;
//         if (splitSize < 2 * options.minLeafSize)
//             return false; // The leaf is too small

//         int splitPoint = GetSplitPoint(
//             splitSize,
//             options,
//             options.splitMinimalLeaves ? (Random.value < 0.5f ? 0 : 1) : null);

//         // size + 1 because the new leaves should use
//         // the same wall in the future
//         if (isVerticalSplit)
//         {
//             int rightChildWidth = leaf.width - splitPoint;

//             leaf.leftChild = new BSPLeaf(leaf.x, leaf.y, splitPoint + 1, leaf.height);
//             leaf.rightChild = new BSPLeaf(
//                 leaf.x + splitPoint,
//                 leaf.y,
//                 rightChildWidth,
//                 leaf.height);
//         }
//         else
//         {
//             int rightChildHeight = leaf.height - splitPoint;

//             leaf.leftChild = new BSPLeaf(leaf.x, leaf.y, leaf.width, splitPoint + 1);
//             leaf.rightChild = new BSPLeaf(
//                 leaf.x,
//                 leaf.y + splitPoint,
//                 leaf.width,
//                 rightChildHeight);
//         }
//         return true; // The split is completed
//     }

//     public static Graph<RectArea> GenerateRooms(
//         this BSPLeaf leaf,
//         BSPGenerationOptions options)
//     {
//         var roomsConnectivity = new Graph<RectArea>();
//         GenerateRooms(leaf, options, roomsConnectivity);
//         return roomsConnectivity;
//     }

//     /// <param name="position">Value in [0, 1] used for interpolation between min
//     /// and max. Use Null for random</param>
//     private static int GetSplitPoint(
//         int splitSize,
//         BSPGenerationOptions options,
//         float? position)
//     {
//         return Mathf.RoundToInt(Mathf.Lerp(
//             options.minLeafSize,
//             splitSize - options.minLeafSize + 1,
//             position ?? Random.value));
//     }

//     /// <summary>
//     /// Generates all the rooms for this Leaf and all its children
//     /// and sets rooms connectivity into a graph is it's passed
//     /// </summary>
//     private static void GenerateRooms(
//         this BSPLeaf leaf,
//         BSPGenerationOptions options,
//         Graph<RectArea> roomsConnectivity = null)
//     {
//         // If the leaf was split
//         if (leaf.leftChild != null || leaf.rightChild != null)
//         {
//             leaf.leftChild?.GenerateRooms(options, roomsConnectivity);
//             leaf.rightChild?.GenerateRooms(options, roomsConnectivity);

//             if (roomsConnectivity != null && leaf.leftChild != null
//                 && leaf.rightChild != null) {
//                 var leftRoom = leaf.leftChild.GetSomeRoom();
//                 var rightRoom = leaf.rightChild.GetSomeRoom();
//                 if (leftRoom != null && rightRoom != null) {
//                     roomsConnectivity.AddLink(leftRoom, rightRoom);
//                 }
//             }
//         }
//         else
//         {
//             // The leaf is ready for creating a room
//             leaf.rect = leaf.CreateRandomRoom(options);
//         }
//     }

//     private static int GenerateRoomSize(
//         int leafSize,
//         BSPGenerationOptions options) {
//         int minSize = leafSize - options.maxRoomOffset;
//         if (minSize < 3)
//             minSize = 3;
//         int maxSize = leafSize - options.minRoomOffset * 2;
//         if (maxSize < minSize)
//             maxSize = minSize;
//         int roomSize = Random.Range(minSize, maxSize + 1);
        
//         int offset = leafSize - roomSize;
//         // Offset should not be 1 to avoid double walls
//         if (offset == 1) {
//             roomSize = leafSize - 2;
//         }

//         return roomSize;
//     }

//     private static RectArea CreateRandomRoom(
//         this BSPLeaf leaf,
//         BSPGenerationOptions options) {
//         int roomWidth = GenerateRoomSize(leaf.width, options);
//         int roomHeight = GenerateRoomSize(leaf.height, options);
//         // Vector2Int roomSize = new Vector2Int(roomWidth, roomHeight);

//         int GenerateRoomPos(int leafSize, int roomSize) {
//             // int res = Random.Range(options.minRoomOffset / 2,
//             //     leafSize - roomSize);
            
//             int res = options.minRoomOffset / 2;
//             // if (options.maxRoomOffset > options.minRoomOffset) {
//             //     res += Random.Range(0, (options.maxRoomOffset
//             //         - options.minRoomOffset) / 2);
//             // }
//             // To avoid double walls
//             // if (res == 1)
//             //     res = 3;
//                 // res = Random.value < 0.5f || roomSize == leafSize ? 0 : 2;
//             return res;
//         }

//         Vector2Int roomPos = new Vector2Int(
//             // Random.Range(0, width - roomSize.x),
//             // Random.Range(0, height - roomSize.y)
//             GenerateRoomPos(leaf.width, roomWidth),
//             GenerateRoomPos(leaf.height, roomHeight)
//             );
        
//         return new RectArea(
//             new RectInt(
//                 leaf.x + roomPos.x,
//                 leaf.y + roomPos.y,
//                 roomWidth,
//                 roomHeight));
//     }
// }
