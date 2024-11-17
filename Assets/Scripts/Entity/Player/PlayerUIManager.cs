using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class PlayerUIManager : NetworkBehaviour
{
    [SerializeField]
    private SoundReceiver soundReceiver;

    [SerializeField]
    private Speaker speaker;

    [SerializeField]
    private AvailableRecipientsUIUpdater availableRecipientsUIUpdater;

    [SerializeField]
    private Person person;

    private CommunicationUIManager communicationUIManager;

    private void Awake() {
        if (new object[] {soundReceiver, speaker, availableRecipientsUIUpdater, person}.Any(x => x == null)) {
            Debug.LogError(
                $"Missing references in {nameof(PlayerUIManager)}. " +
                $"Please, ensure that all the requried serialized field are set.");
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        communicationUIManager = FindObjectOfType<CommunicationUIManager>();

        // availableRecipientsUIUpdater.Initialize();
        InitializeMessagesPanel();
        InitializeCommunicationPanel();
    }

    private void InitializeMessagesPanel() {
        var messagesPanelUpdater = new MessagesPanelUpdater();
        messagesPanelUpdater.Initialize(
            person,
            soundReceiver,
            communicationUIManager.MessagesPanel);
    }

    private void InitializeCommunicationPanel() {
        var choicesProvider = new DemoChoicesProvider(person, speaker);
        communicationUIManager
            .CommunicationPanel
            .Init(choicesProvider, person, speaker);
    }
}
