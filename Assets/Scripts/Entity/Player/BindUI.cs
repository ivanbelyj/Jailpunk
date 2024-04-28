using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoundReceiver))]
[RequireComponent(typeof(Speaker))]
[RequireComponent(typeof(AvailableRecipientsUIUpdater))]
[RequireComponent(typeof(Person))]
public class BindUI : MonoBehaviour
{
    private CommunicationUIManager communicationUIManager;

    private void Start() {
        communicationUIManager = FindObjectOfType<CommunicationUIManager>();

        InitializeMessagesPanel();
        InitializeCommunicationPanel();
    }

    private void InitializeMessagesPanel() {
        var messagesPanelUpdater = new MessagesPanelUpdater();
        messagesPanelUpdater.Initialize(
            GetComponent<Person>(),
            GetComponent<SoundReceiver>(),
            communicationUIManager.MessagesPanel);
    }

    private void InitializeCommunicationPanel() {
        var playersPerson = GetComponent<Person>();
        var speaker = GetComponent<Speaker>();
        var choicesProvider = new DemoChoicesProvider(playersPerson, speaker);
        communicationUIManager
            .CommunicationPanel
            .Init(choicesProvider, playersPerson, speaker);
    }
}
