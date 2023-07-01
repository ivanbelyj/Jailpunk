// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// /// <summary>
// /// A component that allows you to easily calculate the position in the grid
// /// </summary>
// public class GridPositioned : MonoBehaviour
// {
//     [SerializeField]
//     private Grid grid;

//     public Vector3Int CellPosition => GetCellPosition(transform.position);

//     /// <summary>
//     /// Coordinates of cell center in world
//     /// </summary>
//     public Vector3 CellInWorld => GetCellInWorld(CellPosition);

//     public Vector3Int GetCellPosition(Vector3 pos) {
//         return grid.WorldToCell(transform.position);
//     } 
//     public Vector3 GetCellInWorld(Vector3Int cellPos)
//         => grid.GetCellCenterWorld(cellPos);
// }
