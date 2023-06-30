using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IsometricMovement))]
public class Door : MazeObject, IRefreshable
{
    public void Open() {

    }

    public void Close() {

    }

    public override void Activate()
    {
        Open();   
    }

    public void Refresh()
    {
        Close();
    }
}
