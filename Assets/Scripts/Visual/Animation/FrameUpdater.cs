using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameUpdater
{
    private readonly float secondsPerFrame;
    private readonly SpriteLibraryDecorator spriteLibraryDecorator;
    private readonly SpriteRenderer spriteRenderer;
    private AnimationParameters currentAnimation;
    private readonly Vector3 initialLocalScale;
    // private bool isFlipped = false;

    private int currentFrame = -1;
    private float newFrameStartTime;

    public FrameUpdater(
        float secondsPerFrame,
        SpriteLibraryDecorator spriteLibraryDecorator,
        SpriteRenderer spriteRenderer)
    {
        this.secondsPerFrame = secondsPerFrame;
        this.spriteLibraryDecorator = spriteLibraryDecorator;
        this.spriteRenderer = spriteRenderer;

        initialLocalScale = spriteRenderer.transform.localScale;

        currentAnimation = new AnimationParameters();
    }

    public void Update() {
        var categoryAndLabel = TryGetCurrentCategoryAndLabel();
        if (categoryAndLabel == null) {
            return;
        }
        string category = categoryAndLabel.Value.Item1;
        string label = categoryAndLabel.Value.Item2;
        
        if (Time.time >= newFrameStartTime) {
            currentFrame++;
            if (currentFrame >= spriteLibraryDecorator.LabelsCount(category))
                currentFrame = 0;
            newFrameStartTime += secondsPerFrame;

            // Debug.Log($"Next frame {category} {label}");

            spriteRenderer.sprite = spriteLibraryDecorator.GetSprite(category, label);
            spriteRenderer.flipX = currentAnimation.Flip;

            // spriteResolver.SetCategoryAndLabel(category, label);
            // spriteResolver.ResolveSpriteToSpriteRenderer();
        }
    }

    public void SetCurrentAnimation(AnimationParameters newAnimation) {
        AnimationParameters oldAnimation = currentAnimation;
        currentAnimation = newAnimation;
        if (oldAnimation != currentAnimation) {
            currentFrame = 0;
            newFrameStartTime = Time.time + secondsPerFrame;
        }

        // string oldAnimation = currentAnimation.AnimationBase;
        // currentAnimation.AnimationBase = newAnimation.AnimationBase;
        // if (oldAnimation != currentAnimation.AnimationBase) {
        //     currentAnimation.Angle = newAnimation.Angle;
        //     currentFrame = 0;
        //     newFrameStartTime = Time.time + secondsPerFrame;
        // }
    }

    // /// <summary>
    // /// If there's no 270-angle animation, trying to use flipped 90.
    // /// </summary>
    // /// <returns>
    // private string HandleFlip() {
    //     string alterCategory = GetCategoryName(90);
    //     if (currentAnimation.Angle == 270
    //         && spriteLibraryDecorator.HasCategory(alterCategory)) {
    //         spriteRenderer.flipX = true;
    //         return alterCategory;
    //     }

    //     spriteRenderer.flipX = false;
    //     return null;
    // }

    private (string, string)? TryGetCurrentCategoryAndLabel() {
        if (currentFrame < 0)
            return null;

        string category = spriteLibraryDecorator.GetCategoryOrNull(currentAnimation);

        if (category == null) {
            return null;
        }
        
        string[] labels = spriteLibraryDecorator.GetLabels(category);

        if (currentFrame >= labels.Length)
            return null;

        return (category, labels[currentFrame]);
    }
}
