using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zor.UtilityAI.Core;

public class TestAction : Action, ISetupable<object>
{
    protected override void OnInitialize()
    {
        base.OnInitialize();
        Debug.Log("Initialize action");
    }

    protected override void OnBegin()
    {
        base.OnBegin();
        Debug.Log("Action becomes active");
    }

    protected override void OnEnd()
    {
        base.OnEnd();
        Debug.Log("Action becomes inactive");
    }

    public void Setup(object arg)
    {
        Debug.Log("Setup TestAction");
    }
}
