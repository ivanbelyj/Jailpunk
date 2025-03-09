using System.Collections.Generic;
using System.Linq;

public class SchemePosition
{
    public int? SectorId { get; set; }
    public int? AreaId { get; set; }
    public List<SchemeTile> Layers { get; set; } = new();
    public SchemePositionType? Type { get; set; }

    /// <summary>
    /// O(n)
    /// </summary>
    public SchemeTile GetLayerByName(string name) {
        return Layers.Find(x => x.LayerName == name);
    }
}
