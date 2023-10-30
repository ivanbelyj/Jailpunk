using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoundReceiver))]
[RequireComponent(typeof(Speaker))]
[RequireComponent(typeof(AvailableRecipientsUIUpdater))]
[RequireComponent(typeof(Person))]
public class BindUI : MonoBehaviour
{
    private void Start() {
        var communicationUIManager = FindObjectOfType<CommunicationUIManager>();
        communicationUIManager.MessagesPanel
            .AddSoundReceiver(GetComponent<SoundReceiver>());
        communicationUIManager.CommunicationPanel
            .SetSubject(GetComponent<Person>(), GetComponent<Speaker>());
    }
}
