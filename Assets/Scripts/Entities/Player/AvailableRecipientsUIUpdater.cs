using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Speaker))]
[RequireComponent(typeof(Character))]
[RequireComponent(typeof(CharacterControls))]
public class AvailableRecipientsUIUpdater : MonoBehaviour
{
    private SoundEmitter soundEmitter;

    private CommunicationUIManager communicationUIManager;
    private CharacterId subjectId;
    private CharacterControls characterControls;
    // private List<ISubject> lastAvailableRecipients;

    private void Start() {
        subjectId = GetComponent<Character>().GetCharacterId();
        Debug.Log("SubjectId is " + subjectId);
        characterControls = GetComponent<CharacterControls>();

        communicationUIManager = FindObjectOfType<CommunicationUIManager>();
        soundEmitter = GetComponent<Speaker>().SoundEmitter;

        soundEmitter.RadiusChecker.NewInRadius += UpdateAvailableRecipients;
        soundEmitter.RadiusChecker.OutOfRadius += UpdateAvailableRecipients;
    }

    private void UpdateAvailableRecipients(List<GameObject> objects) {
        var recipients = soundEmitter.GetReceiversAndIntensities()
            .Select(x => x.Item1.GetComponent<Character>())
            .Where(x => x != null &&
                x.GetCharacterId() != subjectId)
            .Select(subject => subject.GetCharacterId());
            
        // lastAvailableRecipients = recipients.ToList();
        
        communicationUIManager
            .CommunicationPanel
            .SetAvailableRecipientIds(recipients.ToList());
    }

    private void OnDrawGizmos() {
        Color prevCol = Gizmos.color;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position,
            (Vector2)transform.position
                + characterControls.Orientation
                * soundEmitter.RadiusChecker.Radius);
        Gizmos.color = prevCol;
    }

    // private void Update() {
    //     var hits = Physics2D.RaycastAll(transform.position,
    //         characterControls.Orientation, soundEmitter.RadiusChecker.Radius);
        
    //     float minDistance = float.MaxValue;
    //     ISubject nearestRecipient = null;
    //     foreach (var hit in hits) {
    //         float currentDistance = Vector2
    //             .Distance(transform.position, hit.transform.position);
    //         var recipient = hit.collider.gameObject.GetComponent<ISubject>();
    //         if (recipient != null && currentDistance < minDistance) {
    //             minDistance = currentDistance;
    //             nearestRecipient = recipient;
    //         }
    //     }
        
    //     if (nearestRecipient != null)
    //         communicationUIManager
    //             .CommunicationPanel
    //             .SetCurrentRecipient(nearestRecipient);
    // }

    
    private void UpdateNotUsed()
    {
        // Todo:
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,
            soundEmitter.RadiusChecker.Radius);
        if (colliders.Length == 0)
            return;

        Character nearestRecipient = null;
        // float maxDot = float.MinValue;
        float minDistance = float.MaxValue;

        foreach (Collider2D col in colliders)
        {
            // Vector2 directionToCol = col.transform.position
            //     - transform.position;
            // float dot = Vector2.Dot(characterControls.Orientation,
            //     directionToCol.normalized);
            float distance = Vector2.Distance(transform.position,
                col.transform.position);

            var recipient = col.gameObject.GetComponent<Character>();
            if (recipient != null && distance < minDistance
                && subjectId != recipient.GetCharacterId()
                /*&& dot > 0.5f && dot > maxDot*/)
            {
                nearestRecipient = recipient;
                minDistance = distance;
                // maxDot = dot;
            }
        }

        if (nearestRecipient != null) {
            communicationUIManager
                .CommunicationPanel
                .SetRecipientId(nearestRecipient.GetCharacterId());
        }
            
    }
}
