using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimationParametizer
{
    private readonly SpriteLibraryDecorator spriteLibraryDecorator;
    private int lastAngle;
    private bool lastIsFlipped;

    public AnimationParametizer(SpriteLibraryDecorator spriteLibraryDecorator)
    {
        this.spriteLibraryDecorator = spriteLibraryDecorator;
    }

    public AnimationParameters GetParameters(Vector2 moveInput) {
        string animationBase;
        if (Mathf.Approximately(moveInput.sqrMagnitude, 0f)) {
            animationBase = "idle";
        } else {
            animationBase = "walk";

            // Vector2 normalizedInput = new Vector2(SignOrApproxZero(moveInput.x),
            //     SignOrApproxZero(moveInput.y));
            float signedAngle = -1 * Vector2.SignedAngle(Vector2.up, moveInput);
            lastAngle = AdjustToSupportedAngle(
                Mathf.RoundToInt(signedAngle < 0 ? 360 + signedAngle : signedAngle));
        }

        var res = new AnimationParameters() {
            Angle = lastAngle,
            AnimationBase = animationBase,
        };
        if (ShouldFlip(res)) {
            res.Flip = true;
            res.Angle = 90;
        }
        return res;
    }

    /// <summary>
    /// There are only 4 animation angles are supported, so different angles
    /// should be adjusted
    /// </summary>
    private int AdjustToSupportedAngle(int angle) {
        return angle switch {
            > 0 and < 90 => 90,
            > 90 and < 180 => 90,
            > 180 and < 270 => 270,
            > 270 and < 360 => 270,
            >= 0 and < 360 => angle,
            _ => throw new ArgumentOutOfRangeException(
                nameof(angle),
                "Angle must be in 0 (inclusive), 360 (exclusive)")
        };
    }

    private bool ShouldFlip(AnimationParameters parameters)
    {
        bool result = parameters.Angle == 270
            && !spriteLibraryDecorator.HasCategoryForParameters(parameters)
            && spriteLibraryDecorator.HasCategoryForParameters(parameters, 90);

        return result;
    }
}
