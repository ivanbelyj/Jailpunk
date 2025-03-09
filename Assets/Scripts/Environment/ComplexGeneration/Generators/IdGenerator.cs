using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Todo: use DI instead of Singleton
public sealed class IdGenerator
{
    private static IdGenerator instance = null;
    private int lastId = 0;

    public static IdGenerator Instance
    {
        get
        {
            instance ??= new IdGenerator();
            return instance;
        }
    }

    public int NewSectorId() => ++lastId;
    public int NewAreaId() => ++lastId;

    #if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Initialize()
    {
        Debug.Log("INITIALIZE");
        ResetState();
    }
    
    private static void ResetState()
    {
        instance = null;
    }
    #endif
}
