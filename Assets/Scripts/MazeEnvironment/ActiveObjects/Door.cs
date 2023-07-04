using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
    public void Open() {
        Debug.Log("Door is opened");
    }

    public void Close() {

    }

    public override void Activate()
    {
        Open();   
    }
}
