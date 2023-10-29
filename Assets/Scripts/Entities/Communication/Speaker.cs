using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Some object that can speak or reproduce words
/// </summary>
[RequireComponent(typeof(SoundEmitter))]
public class Speaker : MonoBehaviour
{
    private SoundEmitter soundEmitter;

    public float voiceVolumeMultiplier = 1f;

    private void Awake() {
        soundEmitter = GetComponent<SoundEmitter>();
        soundEmitter.defaultSoundIntensity
            = SpeechVolumeToSoundIntensity(SpeechVolume.Normal);
    }

    public void Speak(SpeechSoundData speechSoundData, SpeechVolume volume) {
        soundEmitter.Emit(speechSoundData, SpeechVolumeToSoundIntensity(volume));
    }

    public float SpeechVolumeToSoundIntensity(SpeechVolume speechVolume) {
        return speechVolume switch {
            SpeechVolume.Whisper => 5f,
            SpeechVolume.Normal => 100f,
            SpeechVolume.Shout => 10_000f,
            _ => throw new ArgumentException("Unknown speech volume")
        } * voiceVolumeMultiplier;
    }
}
