using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

/// <summary>
/// An entity considered as a character with its own identity
/// </summary>
public class Character : NetworkBehaviour
{
    public CharacterId GetCharacterId()
    {
        return new CharacterId(netIdentity.netId);
    }
}
