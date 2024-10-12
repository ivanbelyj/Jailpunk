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

    [SerializeField]
    private bool writeDebugMessages = false;

    private void Awake() {
        appearance = GetComponent<IAppearance>();
        frameUpdater = new FrameUpdater(appearance);
        animationParametizer = new AnimationParametizer(appearance.Schema);
    }

    public void SetMoveInput(Vector2 moveInput) {
        if (writeDebugMessages) {
            Debug.Log(moveInput);
        }
            
        frameUpdater.SetCurrentAnimation(
            animationParametizer.GetParameters(moveInput, frameUpdater.CurrentFrame));
    }

    private void Update() {
        frameUpdater.Update();
    }
}
