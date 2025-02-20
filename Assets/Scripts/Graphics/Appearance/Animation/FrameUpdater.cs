using UnityEngine;

public class FrameUpdater
{
    private AppearanceAnimationData currentAnimation;

    // private int currentFrame = -1;
    public int CurrentFrame => currentAnimation.Frame;
    private float newFrameStartTime;
    private readonly IAppearance appearance;
    private AnimationStateSchema lastStateSchema;
    private bool isSingleFrameRenderedOnce = false;

    public FrameUpdater(IAppearance appearance)
    {
        currentAnimation = new AppearanceAnimationData();
        this.appearance = appearance;
    }

    public void Update() {
        if (!appearance.IsInitialized) {
            return;
        }

        var stateSchema = GetStateSchema(currentAnimation.State);
        if (stateSchema.framesCount == 1) {
            if (!isSingleFrameRenderedOnce) {
                isSingleFrameRenderedOnce = true;
                currentAnimation.Frame = 0;
                Debug.Log("FIRST RENDER " + currentAnimation);
                Render();
            }
            return;
        }

        if (Time.time >= newFrameStartTime) {
            if (stateSchema.animationType == AnimationType.Loop
                || stateSchema.animationType == AnimationType.Once
                && currentAnimation.Frame < stateSchema.framesCount - 1) {
                currentAnimation.Frame++;
            }
            
            if (currentAnimation.Frame >= stateSchema.framesCount) {
                currentAnimation.Frame = 0;
            }
            newFrameStartTime += stateSchema.secondsPerFrame;

            Render();
        }
    }

    public void SetCurrentAnimation(AppearanceAnimationData newAnimation) {
        AppearanceAnimationData oldAnimation = currentAnimation;
        currentAnimation = newAnimation;
        if (oldAnimation != currentAnimation) {
            isSingleFrameRenderedOnce = false;

            if (currentAnimation.State == AnimationConstants.StateWalk 
                && currentAnimation.LastWalkFrame.HasValue) {
                Debug.Log("Last walk frame is not null!" + currentAnimation.LastWalkFrame);
            }

            currentAnimation.Frame = currentAnimation.LastWalkFrame ?? 0;

            var stateSchema = GetStateSchema(currentAnimation.State);
            newFrameStartTime = Time.time + stateSchema.secondsPerFrame;
        }
    }
    
    private AnimationStateSchema GetStateSchema(string state) {
        if (lastStateSchema == null || lastStateSchema.state != state) {
            lastStateSchema = appearance.Schema.animationSchema.GetStateSchema(state);
        }
        return lastStateSchema;
    }

    private void Render() {
        appearance.Render(currentAnimation);
    }
}
