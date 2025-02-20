using UnityEngine;

public class WalkingAppearanceAnimator : AppearanceAnimator
{
    protected AnimationParametizer animationParametizer;

    protected override void Awake()
    {
        base.Awake();
        animationParametizer = new AnimationParametizer(appearance.Schema);
        SetInitialAnimation();
    }
    
    private void Start() {
        
    }

    public void SetMoveInput(Vector2 moveInput) {   
        SetCurrentAnimation(
            animationParametizer.GetParameters(moveInput, frameUpdater.CurrentFrame));
    }

    private void SetInitialAnimation() {
        SetCurrentAnimation(new() {
            Angle = 180,
            State = AnimationConstants.StateIdle,
        });
    }
}
