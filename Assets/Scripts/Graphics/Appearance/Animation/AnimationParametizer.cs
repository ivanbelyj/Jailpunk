using System;
using UnityEngine;

using static AnimationConstants;

public class AnimationParametizer
{
    private readonly AppearanceSchema appearanceSchema;
    private int lastAngle;
    private string lastState;

    private int? lastWalkFrame;

    public AnimationParametizer(AppearanceSchema appearanceSchema)
    {
        this.appearanceSchema = appearanceSchema;
    }

    public AppearanceAnimationData GetParameters(Vector2 moveInput, int frame) {
        string state;
        if (Mathf.Approximately(moveInput.sqrMagnitude, 0f)) {
            state = StateIdle;
        } else {
            state = StateWalk;

            // Todo: use GridDirectionUtils.VectorToDirection?
            float signedAngle = -1 * Vector2.SignedAngle(Vector2.up, moveInput);
            lastAngle = GridDirectionUtils.AdjustToAngleSupportedByAnimation(
                Mathf.RoundToInt(signedAngle < 0 ? 360 + signedAngle : signedAngle));
        }

        SetLastWalkFrame(state, frame);

        var res = new AppearanceAnimationData() {
            Angle = lastAngle,
            State = state,
            Frame = frame,
            LastWalkFrame = lastWalkFrame
        };
        if (ShouldFlip(res)) {
            res.Flip = true;
            res.Angle = 90;
        }
        return res;
    }

    private void SetLastWalkFrame(string state, int frame) {
        if (lastState == StateWalk && state == StateIdle) {
            lastWalkFrame = frame;
        } else if (state == StateWalk) {
            lastWalkFrame = null;
        }

        lastState = state;
    }

    private bool ShouldFlip(AppearanceAnimationData parameters)
    {
        bool result = parameters.Angle == 270
            && appearanceSchema.useFlipped90For270;

        return result;
    }
}
