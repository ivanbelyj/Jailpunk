using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Person : NetworkBehaviour, ISubject
{
    [SerializeField]
    private string personName;
    public string PersonName {
        get => personName;
        set => personName = value;
    }

    public ulong GetSubjectId()
    {
        return netIdentity.sceneId;
    }

    /// <summary>
    /// Gets the name of some other subject as this person knows
    /// </summary>
    public string GetKnownSubjectName(ulong id) {
        NetworkIdentity identity = null;
        try {
            identity = NetworkIdentity.GetSceneIdentity(id);
        } catch (KeyNotFoundException) {
            Debug.LogWarning("Scene identity not found by " + id);
        }
        
        // Todo:
        return identity?.GetComponent<Person>().PersonName
            ?? id.ToString();
    }
}
