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
        var playersPerson = GetComponent<Person>();

        communicationUIManager.MessagesPanel
            .AddSoundReceiver(GetComponent<SoundReceiver>());
        communicationUIManager.MessagesPanel.SetSubjectPerson(playersPerson);
        communicationUIManager.CommunicationPanel
            .SetSubjectPerson(playersPerson, GetComponent<Speaker>());
    }
}
