using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ApplyAreaHelper
{
    private readonly IdGenerator idGenerator;

    public ApplyAreaHelper(IdGenerator idGenerator)
    {
        this.idGenerator = idGenerator;
    }

    public List<SchemeArea> ToSchemeAreas(List<BSPNode> bspNodes) {
        return bspNodes
            .Where(x => x.rectArea != null || x.intermediateArea != null)
            .Select(x => {
                var (type, rect) = GetAreaTypeAndRect(x);
                return new SchemeArea() {
                    Id = idGenerator.NewAreaId(),
                    Type = type,
                    Rect = rect
                };
            })
            .ToList();

        (SchemeAreaType, RectInt) GetAreaTypeAndRect(BSPNode node) {
            bool hasIntermediateArea = node.intermediateArea != null;
            return hasIntermediateArea
                ? (SchemeAreaType.Corridor, node.intermediateArea.Rect)
                : (SchemeAreaType.Room, node.rectArea.Rect);
        }
    }

    public void ApplyToScheme(ComplexScheme scheme, SchemeArea area) {
        TraverseRectUtils.TraverseRect(area.Rect, (data) => {
            bool isBorder = data.isBorder;
            int x = data.x;
            int y = data.y;
            
            var tile = scheme.GetTileByPos(x, y);
            tile.AreaId = area.Id;
            if (area.Type == SchemeAreaType.Room
                && isBorder
                && tile.TileType != TileType.LoadBearingWall)
            {
                tile.TileType = TileType.Wall;
            }
        });
    }
}
