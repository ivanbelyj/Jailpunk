using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharacterId : IEquatable<CharacterId>
{
    [SerializeField]
    private uint value;
    public uint Value => value;

    public bool IsNoIdentity => Value == 0;
    public bool IsCharacterIdentity => Value != 0;

    public static CharacterId NoIdentity => new CharacterId(0);

    public CharacterId(uint value) {
        this.value = value;
    }

    public override bool Equals(object obj)
    {
        return obj is CharacterId other && Equals(other);
    }

    public bool Equals(CharacterId other)
    {
        return other.value == value;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(value);
    }

    public static bool operator ==(CharacterId id1, CharacterId id2)
        => id1.value == id2.value;
    public static bool operator !=(CharacterId id1, CharacterId id2)
        => id1.value != id2.value;

    public override string ToString()
    {
        return value.ToString();
    }
}
