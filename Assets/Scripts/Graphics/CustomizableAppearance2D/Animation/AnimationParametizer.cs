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

    public AnimationParameters GetParameters(Vector2 moveInput, int frame) {
        string state;
        if (Mathf.Approximately(moveInput.sqrMagnitude, 0f)) {
            state = StateIdle;
        } else {
            state = StateWalk;

            float signedAngle = -1 * Vector2.SignedAngle(Vector2.up, moveInput);
            lastAngle = AdjustToSupportedAngle(
                Mathf.RoundToInt(signedAngle < 0 ? 360 + signedAngle : signedAngle));
        }

        SetLastWalkFrame(state, frame);

        var res = new AnimationParameters() {
            Angle = lastAngle,
            State = state,
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

    /// <summary>
    /// There are only 4 animation angles are supported, so different angles
    /// should be adjusted
    /// </summary>
    private int AdjustToSupportedAngle(int angle) {
        return angle switch {
            >= 0 and < 35 => 0, // 45
            >= 35 and <= 145 => 90,
            > 145 and < 215 => 180, // 135, 225
            >= 215 and <= 325 => 270,
            > 325 and <= 360 => 0, // 315
            _ => throw new ArgumentOutOfRangeException(
                $"Invalid {nameof(angle)} value: {angle}. " +
                "Angle must be in 0 (inclusive), 360 (inclusive)")
        };
    }

    private bool ShouldFlip(AnimationParameters parameters)
    {
        bool result = parameters.Angle == 270
            && appearanceSchema.useFlipped90For270;

        return result;
    }
}
