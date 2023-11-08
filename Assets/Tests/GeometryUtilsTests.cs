using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GeometryUtilsTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestIntersectWithBreadth()
    {
        // 01234567
        // -----
        //   -----
        Assert.True(GeometryUtils.IntersectWithBreadth(0, 4, 2, 6, 3));

        // 01234567
        // -----
        //    -----
        Assert.False(GeometryUtils.IntersectWithBreadth(0, 4, 3, 7, 3));

        // 01234567
        //   -----
        // -----
        Assert.True(GeometryUtils.IntersectWithBreadth(0, 4, 2, 6, 3));

        // 01234567
        //    -----
        // -----
        Assert.False(GeometryUtils.IntersectWithBreadth(0, 4, 3, 7, 3));

        // 01234567
        // -----
        // -----
        Assert.True(GeometryUtils.IntersectWithBreadth(0, 4, 0, 4, 3));
    }

    [Test]
    public void TestGetIntersectionSegment() {
        // 01234567
        // -----
        //   -----
        Assert.AreEqual((2, 4), GeometryUtils.GetIntersectionSegment(0, 4, 2, 6));

        // 01234567
        // -----
        //    -----
        Assert.AreEqual((3, 4), GeometryUtils.GetIntersectionSegment(0, 4, 3, 7));

        // 01234567
        //   -----
        // -----
        Assert.AreEqual((2, 4), GeometryUtils.GetIntersectionSegment(0, 4, 2, 6));

        // 01234567
        //    -----
        // -----
        Assert.AreEqual((3, 4), GeometryUtils.GetIntersectionSegment(0, 4, 3, 7));

        // 01234567
        // -----
        // -----
        Assert.AreEqual((0, 4), GeometryUtils.GetIntersectionSegment(0, 4, 0, 4));
    }
}
