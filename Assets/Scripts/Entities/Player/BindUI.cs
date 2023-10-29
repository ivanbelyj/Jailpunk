using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoundReceiver))]
public class BindUI : MonoBehaviour
{
    private void Start() {
        var communicationUIManager = FindObjectOfType<CommunicationUIManager>();
        communicationUIManager.MessagesPanel
            .AddSoundReceiver(GetComponent<SoundReceiver>());
    }
}
