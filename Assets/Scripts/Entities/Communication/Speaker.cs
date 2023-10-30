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
    public SoundEmitter SoundEmitter => soundEmitter;

    public float voiceVolumeMultiplier = 1f;
    public SpeechVolume defaultSpeechVolume = SpeechVolume.Normal;

    private void Awake() {
        soundEmitter = GetComponent<SoundEmitter>();
        soundEmitter.SoundIntensity
            = GetSoundIntensity(SpeechVolume.Normal);
    }

    public void Speak(SpeechSoundData speechSoundData,
        SpeechVolume? volume = null) {
        soundEmitter.SoundIntensity = GetSoundIntensity(volume);
        soundEmitter.Emit(speechSoundData);
    }

    public float GetSoundIntensity(SpeechVolume? speechVolume = null) {
        if (speechVolume == null)
            speechVolume = defaultSpeechVolume;

        return speechVolume switch {
            SpeechVolume.Whisper => 5f,
            SpeechVolume.Normal => 100f,
            SpeechVolume.Shout => 10_000f,
            _ => throw new ArgumentException("Unknown speech volume")
        } * voiceVolumeMultiplier;
    }
}
