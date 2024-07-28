using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class SpriteSwapAnimator : MonoBehaviour
{
    [SerializeField]
    private SpriteLibrary spriteLibrary;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float secondsPerFrame = 0.2f;

    private SpriteLibraryDecorator spriteLibraryDecorator;
    private FrameUpdater frameUpdater;
    private AnimationParametizer animationParametizer;

    [SerializeField]
    private bool writeDebugMessages = false;

    private void Awake() {
        spriteLibraryDecorator = new SpriteLibraryDecorator(spriteLibrary.spriteLibraryAsset);
        frameUpdater = new FrameUpdater(
            secondsPerFrame,
            spriteLibraryDecorator,
            spriteRenderer);
        animationParametizer = new AnimationParametizer(spriteLibraryDecorator);
    }

    public void SetMoveInput(Vector2 moveInput) {
        if (writeDebugMessages) {
            Debug.Log(moveInput);
        }
            
        frameUpdater.SetCurrentAnimation(animationParametizer.GetParameters(moveInput));
    }

    private void Update() {
        frameUpdater.Update();
    }
}
