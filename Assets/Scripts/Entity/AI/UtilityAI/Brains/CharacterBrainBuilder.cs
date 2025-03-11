using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zor.SimpleBlackboard.Core;
using Zor.UtilityAI.Builder;
using Zor.UtilityAI.Core;
using Zor.UtilityAI.Core.Actions;
using Zor.UtilityAI.Core.Considerations;

public class CharacterBrainBuilder
{
    public Brain Builder(Blackboard blackboard, BrainSettings brainSettings) {
        var builder = new BrainBuilder();

        builder.AddAction<TestAction, object>(new object(), "TestAction");
        builder.AddConsideration<LinearConsideration, float, float, float, string>(
            1f, 0f, 1f, "LinearConsideration");

        builder.AddAction<DoNothingAction>("DoNothingAction");
        builder.AddConsideration<ConstantConsideration, float>(
            1f, "ConstantConsideration");

        return builder.Build(blackboard, brainSettings);
    }
}
