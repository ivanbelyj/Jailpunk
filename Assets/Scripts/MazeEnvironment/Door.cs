using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(IsometricMovement))]
public class Door : ActivatableObject, IRefreshable
{
    // private IsometricMovement movement;
    // private void Awake() {
    //     movement = GetComponent<IsometricMovement>();
    // }

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
