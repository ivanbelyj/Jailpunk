using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundReceiver : MonoBehaviour
{
    public float soundThreshold;

    public delegate void SoundReceivedEventHandler(object sender,
        SoundReceivedEventArgs e);

    public class SoundReceivedEventArgs {
        public float Intensity { get; set; }
        public Vector3 Position { get; set; }
        public SoundData SoundData { get; set; }
    }

    public event SoundReceivedEventHandler SoundReceived;

    public virtual void Receive(float intensity, Vector3 position,
        SoundData soundData) {
        SoundReceived?.Invoke(this, new SoundReceivedEventArgs() {
            Intensity = intensity,
            Position = position,
            SoundData = soundData
        });
    }
}
