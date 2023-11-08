using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CorridorsGeneratorTests
{
    private RectArea TopLeftArea => new RectArea(0, 0, 20, 20);
    private RectArea TopRightArea => new RectArea(100, 0, 20, 20);
    private RectArea BottomLeftArea => new RectArea(100, 0, 20, 20);
    private RectArea BottomRightArea => new RectArea(100, 100, 20, 20);
    private RectArea CenterArea => new RectArea(40, 40, 20, 20);

    private RectArea GetWithOffset(RectArea area, int offsetX, int offsetY) {
        area.Rect = new RectInt(area.Rect.position
            + new Vector2Int(offsetX, offsetY), area.Rect.size);
        return area;
    }

    [Test]
    public void TestGetTwoConnectablePoints()
    {
        CorridorsGenerator gen = new CorridorsGenerator();
        
    }
}
