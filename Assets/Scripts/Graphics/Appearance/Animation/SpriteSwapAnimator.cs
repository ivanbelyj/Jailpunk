using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(IAppearance))]
public class SpriteSwapAnimator : MonoBehaviour
{
    private IAppearance appearance;

    private FrameUpdater frameUpdater;
    private AnimationParametizer animationParametizer;
    
    private void Awake() {
        appearance = GetComponent<IAppearance>();
        frameUpdater = new FrameUpdater(appearance);
        animationParametizer = new AnimationParametizer(appearance.Schema);
    }

    private void Start() {
        SetInitialAnimation();
    }

    private void SetInitialAnimation() {
        frameUpdater.SetCurrentAnimation(new() {
            Angle = 180,
            State = AnimationConstants.StateIdle,
        });
    }

    public void SetMoveInput(Vector2 moveInput) {   
        frameUpdater.SetCurrentAnimation(
            animationParametizer.GetParameters(moveInput, frameUpdater.CurrentFrame));
    }

    private void Update() {
        frameUpdater.Update();
    }
}
