using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Speaker))]
public class PeriodicSpeaker : MonoBehaviour
{
    private float timeNextSound;
    public float period = 2f;
    private Speaker speaker;

    private void Awake() {
        speaker = GetComponent<Speaker>();
    }

    private void Start() {
        timeNextSound = Time.time;
    }

    private int counter = 0;

    private const string testText = "Layout controllers are components that control the sizes and possibly positions of one or more layout elements, meaning Game Objects with Rect Transforms on. A layout controller may control its own layout element (the same Game Object it is on itself) or it may control child layout elements.";

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= timeNextSound) {
            timeNextSound = Time.time + period;
            speaker.Speak(new SpeechSoundData() {
                Message =  counter % 2 == 0 ? "Бегемот." : testText
            }, SpeechVolume.Normal);
            counter++;
        }
    }
}