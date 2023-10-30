using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SpeechSoundData : SoundData
{
    public string Message { get; set; }

    /// <summary>
    /// In real life, the recipient is not always determined by speech,
    /// but let's assume that the character addressed
    /// the speech to a certain recipient via some other ways.
    /// 0 - recipient is not defined
    /// </summary>
    public CharacterId RecipientId { get; set; }

    /// <summary>
    /// 0 - speaker's identity is not defined
    /// </summary>
    public CharacterId SpeakerId { get; set; }

    public override string ToDisplayText()
    {
        return $"{Message}";
    }
    // There also can be different speech signs
    // gender, legibility, etc.
}
