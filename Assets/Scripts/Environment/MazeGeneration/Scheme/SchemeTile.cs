using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchemeTile
{
    /// <summary>
    /// Every tile belongs to the certain sector.
    /// </summary>
    public int? SectorId { get; set; }
    public TileType TileType { get; set; }
    public uint TileId { get; set; }
}
