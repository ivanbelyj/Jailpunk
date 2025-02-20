using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagesPanelUpdater
{
    private MessagesPanel messagesPanel;
    private Person subjectPerson;

    public void Initialize(
        Person person,
        SoundReceiver soundReceiver,
        MessagesPanel messagesPanel) {
        this.messagesPanel = messagesPanel;
        SetSubjectPerson(person);
        AddSoundReceiver(soundReceiver);
    }

    private void SetSubjectPerson(Person person) {
        subjectPerson = person;
    }

    private void AddSoundReceiver(SoundReceiver soundReceiver) {
        soundReceiver.SoundReceived += OnSoundReceived;
    }

    private void OnSoundReceived(
        object sender,
        SoundReceiver.SoundReceivedEventArgs args) {
        
        string messageText = "";
        if (args.SoundData is SpeechSoundData speech) {
            messageText += subjectPerson.GetKnownCharacterName(speech.SpeakerId)
                + ": ";
        }
        
        messageText += args.SoundData.ToDisplayText();

        messagesPanel.AddOrUpdateMessage(args.SoundData.SoundId, messageText);
        messagesPanel.ScrollToBottom();
    }
}
