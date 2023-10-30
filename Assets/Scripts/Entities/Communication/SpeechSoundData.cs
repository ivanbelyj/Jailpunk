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
    /// the speech to a certain recipient via some other ways
    /// </summary>
    public ulong RecipientId { get; set; }

    // Todo: 
    public override string ToDisplayText()
    {
        // Todo: get speaker's name if it's known
        string name = RecipientId == 0 ? null : NetworkIdentity
            .GetSceneIdentity(RecipientId)?.gameObject.name;
        string res = $"{Message}";
        if (name != null)
            res = name + ": " + res;
        return res;
    }
    // There also can be different speech signs
    // gender, legibility, etc.
}
