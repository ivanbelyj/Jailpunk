using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Person))]
[RequireComponent(typeof(SoundReceiver))]
[RequireComponent(typeof(Speaker))]
public class TestAnswerer : MonoBehaviour
{
    private Person subject;
    private Speaker speaker;
    private void Awake() {
        subject = GetComponent<Person>();
        speaker = GetComponent<Speaker>();
        
        GetComponent<SoundReceiver>().SoundReceived += (sender, args) => {
            if (args.SoundData is SpeechSoundData speech) {
                string knownName = subject.GetKnownCharacterName(speech.SpeakerId);
                if (speech.RecipientId == subject.GetCharacterId()) {
                    StartCoroutine(DelayedSpeakCoroutine(speech.SpeakerId, 
                        $"Hey, {knownName}, I hear that you asked to me! "
                        + "Peaneim!"));
                }
                if (speech.SpeakerId.IsCharacterIdentity &&
                    speech.RecipientId.IsNoIdentity) {
                    StartCoroutine(DelayedSpeakCoroutine(speech.SpeakerId, 
                        "Hey, come to me! May be I can help you"));
                }
            }
        };
    }

    private IEnumerator DelayedSpeakCoroutine(CharacterId speakerId,
        string message, float delayInSeconds = 0.6f)
    {
        yield return new WaitForSeconds(delayInSeconds);
        speaker.Speak(GetAnswer(speakerId, message));
    }

    private SpeechSoundData GetAnswer(CharacterId characterId, string message) {
        return new SpeechSoundData() {
            SpeakerId = subject.GetCharacterId(),
            RecipientId = characterId,
            Message = message
        };
    }
}
