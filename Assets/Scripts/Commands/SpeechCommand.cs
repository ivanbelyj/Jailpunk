using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechCommand : CommunicationCommand
{
    public Speaker Speaker { get; set; }
    public SpeechSoundData SpeechSoundData { get; set; }
    public SpeechVolume SpeechVolume { get; set; }

    public override void Execute()
    {
        Speaker.Speak(SpeechSoundData, SpeechVolume);
    }

    public override string GetDisplayText()
    {
        return $"{SpeechSoundData.Message}";
    }
}
