using UnityEngine;

public class FrameUpdater
{
    private AnimationParameters currentAnimation;

    private int currentFrame = -1;
    public int CurrentFrame => currentFrame;
    private float newFrameStartTime;
    private readonly IAppearance appearance;

    public FrameUpdater(IAppearance appearance)
    {
        currentAnimation = new AnimationParameters();
        this.appearance = appearance;
    }

    public void Update() {
        if (Time.time >= newFrameStartTime) {
            currentFrame++;

            var stateSchema = GetStateSchema(currentAnimation.State);
            if (currentFrame >= stateSchema.framesCount)
                currentFrame = 0;
            newFrameStartTime += stateSchema.secondsPerFrame;

            Render();
        }
    }

    private AnimationStateSchema GetStateSchema(string state)
        => appearance.Schema.animationSchema.GetStateSchema(state);

    public void SetCurrentAnimation(AnimationParameters newAnimation) {
        AnimationParameters oldAnimation = currentAnimation;
        currentAnimation = newAnimation;
        if (oldAnimation != currentAnimation) {
            if (currentAnimation.State == AnimationConstants.StateWalk 
                && currentAnimation.LastWalkFrame.HasValue) {
                Debug.Log("Last walk frame is not null!" + currentAnimation.LastWalkFrame);
            }

            currentFrame = currentAnimation.LastWalkFrame ?? 0;

            var stateSchema = GetStateSchema(currentAnimation.State);
            newFrameStartTime = Time.time + stateSchema.secondsPerFrame;
        }
    }

    private void Render() {
        appearance.Render(
            new AppearanceRenderData() {
                State = currentAnimation.State,
                Angle = currentAnimation.Angle,
                Frame = currentFrame,
                FlipX = currentAnimation.Flip,
                LastWalkFrame = currentAnimation.LastWalkFrame
            });
    }
}
