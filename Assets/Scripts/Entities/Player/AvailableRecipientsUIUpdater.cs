using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Speaker))]
[RequireComponent(typeof(ISubject))]
public class AvailableRecipientsUIUpdater : MonoBehaviour
{
    private SoundEmitter soundEmitter;

    private CommunicationUIManager communicationUIManager;
    private ISubject subject;

    private void Awake() {
        subject = GetComponent<ISubject>();
    }

    private void Start() {
        communicationUIManager = FindObjectOfType<CommunicationUIManager>();
        soundEmitter = GetComponent<Speaker>().SoundEmitter;

        soundEmitter.RadiusChecker.NewInRadius += UpdateAvailableRecipients;
        soundEmitter.RadiusChecker.OutOfRadius += UpdateAvailableRecipients;
    }

    private void UpdateAvailableRecipients(List<GameObject> objects) {
        var recipients = soundEmitter.GetReceiversAndIntensities()
            .Select(x => x.Item1.GetComponent<ISubject>())
            .Where(recipient => recipient != null
                && recipient.GetSubjectId() != subject.GetSubjectId());
        communicationUIManager
            .CommunicationPanel
            .SetAvailableRecipients(recipients);
    }
}
