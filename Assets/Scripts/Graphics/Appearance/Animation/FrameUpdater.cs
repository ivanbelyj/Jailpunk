using UnityEngine;

public class FrameUpdater
{
    private AppearanceAnimationData currentAnimation;

    // private int currentFrame = -1;
    public int CurrentFrame => currentAnimation.Frame;
    private float newFrameStartTime;
    private readonly IAppearance appearance;

    public FrameUpdater(IAppearance appearance)
    {
        currentAnimation = new AppearanceAnimationData();
        this.appearance = appearance;
    }

    public void Update() {
        if (Time.time >= newFrameStartTime) {
            currentAnimation.Frame++;

            var stateSchema = GetStateSchema(currentAnimation.State);
            if (currentAnimation.Frame >= stateSchema.framesCount) {
                currentAnimation.Frame = 0;
            }
            newFrameStartTime += stateSchema.secondsPerFrame;

            Render();
        }
    }

    private AnimationStateSchema GetStateSchema(string state)
        => appearance.Schema.animationSchema.GetStateSchema(state);

    public void SetCurrentAnimation(AppearanceAnimationData newAnimation) {
        AppearanceAnimationData oldAnimation = currentAnimation;
        currentAnimation = newAnimation;
        if (oldAnimation != currentAnimation) {
            if (currentAnimation.State == AnimationConstants.StateWalk 
                && currentAnimation.LastWalkFrame.HasValue) {
                Debug.Log("Last walk frame is not null!" + currentAnimation.LastWalkFrame);
            }

            currentAnimation.Frame = currentAnimation.LastWalkFrame ?? 0;

            var stateSchema = GetStateSchema(currentAnimation.State);
            newFrameStartTime = Time.time + stateSchema.secondsPerFrame;
        }
    }

    private void Render() {
        appearance.Render(currentAnimation);
    }
}
