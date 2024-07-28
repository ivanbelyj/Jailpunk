using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zor.SimpleBlackboard.Components;
using Zor.UtilityAI.Core;

public class CharacterAgent : MonoBehaviour
{
    private SimpleBlackboardContainer blackboard;
    private Brain brain;

    private void Awake() {
        blackboard = GetComponent<SimpleBlackboardContainer>();
        blackboard.blackboard.SetStructValue<float>(new("testValue"), 10f);
        // Debug.Log("Contains struct value: "
        //     + blackboard.blackboard.ContainsStructValue<float>(new("testValue")));
    }


}
