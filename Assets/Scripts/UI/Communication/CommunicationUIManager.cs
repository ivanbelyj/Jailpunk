using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationUIManager : MonoBehaviour
{
    [SerializeField]
    private MessagesPanel messagesPanel;
    public MessagesPanel MessagesPanel => messagesPanel;

    [SerializeField]
    private CommunicationPanel communicationPanel;
    public CommunicationPanel CommunicationPanel => communicationPanel;

    public void ToggleUI() {
        Debug.Log("Toggle communication UI");
    }
    
}
