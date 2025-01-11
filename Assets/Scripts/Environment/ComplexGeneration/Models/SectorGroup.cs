using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [System.Serializable]
// public class SectorGroup
// {
//     [SerializeField] private int id;
//     [Tooltip("Groups with defined id will be placed as far as possible from this group")]
//     [SerializeField] private int[] alienatedGroupIds;

//     public int Id => id;
//     public int[] AlienatedGroupIds => alienatedGroupIds;
// }

[System.Serializable]
public class AllocatableAreaGroup
{
    [SerializeField] private int id;
    [Tooltip("Groups with defined id will be placed as far as possible from this group")]
    [SerializeField] private int[] alienatedGroupIds;

    public int Id => id;
    public int[] AlienatedGroupIds => alienatedGroupIds;
}
