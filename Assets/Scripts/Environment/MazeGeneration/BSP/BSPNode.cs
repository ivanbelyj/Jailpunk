using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Binary space partition node
/// </summary>
public class BSPNode
{
    public int y;
    public int x;
    public int width;
    public int height;

    public BSPNode leftChild;
    public BSPNode rightChild;
    public RectArea rectArea;

    /// <summary>
    /// In particular, it is used for corridors
    /// </summary>
    public RectArea intermediateArea;

    public BSPNode(
        int x,
        int y,
        int width,
        int height)
    {
       this.x = x;
       this.y = y;
       this.width = width;
       this.height = height;
    }

    public float AspectRatio => width > height
        ? width / (float)height
        : height / (float)width;

    public override int GetHashCode() => (x, y, width, height).GetHashCode();

    public bool IsSplit() => leftChild != null || rightChild != null;    

    /// <summary>
    /// Gets some room from the leaf or from its children
    /// </summary>
    public RectArea GetSomeArea()
    {
        if (rectArea != null) {
            return rectArea;
        }
        else
        {
            RectArea leftRoom = null;
            RectArea rightRoom = null;
            if (leftChild != null)
            {
                leftRoom = leftChild.GetSomeArea();
            }
            if (rightChild != null)
            {
                rightRoom = rightChild.GetSomeArea();
            }
            if (leftRoom == null && rightRoom == null) {
                return null;
            }
            else if (rightRoom == null) {
                return leftRoom;
            }
            else if (leftRoom == null) {
                return rightRoom;
            }
            else if (Random.value > 0.5f) {
                return leftRoom;
            }
            else {
                return rightRoom;
            }
        }
    }
}
