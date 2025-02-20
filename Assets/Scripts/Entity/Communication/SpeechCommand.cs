using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechCommand : CommunicationCommand
{
    private readonly bool useNewSoundIds;

    public Speaker Speaker { get; set; }
    public SpeechSoundData SpeechSoundData { get; set; }
    public SpeechVolume? SpeechVolume { get; set; }

    public SpeechCommand(
        Speaker speaker,
        SpeechSoundData baseSoundData,
        SpeechVolume? speechVolume = null,
        bool useNewSoundIds = true)
    {
        Speaker = speaker;
        SpeechSoundData = baseSoundData;
        SpeechVolume = speechVolume;
        this.useNewSoundIds = useNewSoundIds;
    }

    public override void Execute()
    {
        var soundData = SpeechSoundData with {
            SoundId = useNewSoundIds
                ? Guid.NewGuid()
                : SpeechSoundData.SoundId
        };
        Speaker.Speak(soundData, SpeechVolume);
    }

    public override string GetDisplayText()
    {
        return $"{SpeechSoundData.Message}";
    }
}
