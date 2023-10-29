using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechSoundData : SoundData
{
    public string Message { get; set; }

    // Todo: 
    public override string ToDisplayText()
    {
        return Message;
    }
    // There also can be different speech signs
    // gender, legibility, etc.
}
