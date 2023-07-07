using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject, IRenewable
{
    private bool isOpened;

    public bool IsInRenewedState => isOpened;

    public void Open() {
        if (isOpened)
            return;

        Debug.Log("Door is opened");
        State = ActivationState.ReadyToActivate;  // Door can be closed
        isOpened = true;
    }

    public void Close() {
        if (!isOpened)
            return;
        
        Debug.Log("Door is closed");
        State = ActivationState.ReadyToActivate;
        isOpened = false;
    }

    protected override void Activate()
    {
        if (isOpened)
            Close();
        else
            Open();
    }

    public void Renew()
    {
        Close();
    }
}
