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
    // public SpeechVolume defaultSpeechVolume = SpeechVolume.Normal;
    // private SpeechVolume speechVolume;

    [SerializeField]
    private SpeechVolume initialSpeechVolume = SpeechVolume.Normal;
    public void SetSpeechVolume(SpeechVolume speechVolume) {
        soundEmitter.SoundIntensity = GetSoundIntensity(speechVolume);
    }

    private void Awake() {
        soundEmitter = GetComponent<SoundEmitter>();
        SetSpeechVolume(initialSpeechVolume);
    }

    public void Speak(SpeechSoundData speechSoundData,
        SpeechVolume? volume = null) {
        bool shouldRestorePrevSoundIntensity = false;
        float prevSoundIntensity = soundEmitter.SoundIntensity;
        if (volume != null) {
            shouldRestorePrevSoundIntensity = true;
            soundEmitter.SoundIntensity = GetSoundIntensity(volume.Value);
        }
        
        soundEmitter.Emit(speechSoundData);

        if (shouldRestorePrevSoundIntensity) {
            soundEmitter.SoundIntensity = prevSoundIntensity;
        }
    }

    public float GetSoundIntensity(SpeechVolume speechVolume) {
        return speechVolume switch {
            SpeechVolume.Whisper => 5f,
            SpeechVolume.Normal => 100f,
            SpeechVolume.Shout => 10_000f,
            _ => throw new ArgumentException("Unknown speech volume")
        } * voiceVolumeMultiplier;
    }
}
