using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

[RequireComponent(typeof(SpriteResolver))]
public class SpriteSwapAnimator : MonoBehaviour
{
    [SerializeField]
    private SpriteLibraryAsset spriteLibraryAsset;
    private SpriteResolver spriteResolver;

    private string currentAnimation;
    private int currentAngle;
    private int currentFrame = -1;
    // private string[] currentLabels;

    private float newFrameStartTime;
    [SerializeField]
    private float secondsPerFrame = 0.1f;

    private Dictionary<string, string[]> categoriesAndLabels;
    
    private (string, string)? TryGetCurrentCategoryAndLabel() {
        if (currentFrame < 0)
            return null;

        string category = $"{currentAnimation}-{currentAngle.ToString("D3")}";
        if (!categoriesAndLabels.ContainsKey(category))
            return null;
        string[] labels = categoriesAndLabels[category];

        if (currentFrame >= labels.Length)
            return null;

        return (category, labels[currentFrame]);
    }

    private void Awake() {
        spriteResolver = GetComponent<SpriteResolver>();

        categoriesAndLabels = new Dictionary<string, string[]>();
        foreach (string category in spriteLibraryAsset.GetCategoryNames()) {
            categoriesAndLabels.Add(category,
                spriteLibraryAsset.GetCategoryLabelNames(category).ToArray());
        }
    }

    // private static float SignOrApproxZero(float val) {
    //     if (Mathf.Abs(val) < 0.1f)
    //         return 0f;
    //     else return val < 0 ? -1f : 1f;
    // }


    public void SetMoveInput(Vector2 moveInput) {
        if (Mathf.Approximately(moveInput.sqrMagnitude, 0f))
        {
            SetCurrentAnimation("idle");
        } else {
            SetCurrentAnimation("run");
            // Vector2 normalizedInput = new Vector2(SignOrApproxZero(moveInput.x),
            //     SignOrApproxZero(moveInput.y));
            float signedAngle = -1 * Vector2.SignedAngle(Vector2.up, moveInput);
            currentAngle = Mathf.RoundToInt(signedAngle < 0 ? 360 + signedAngle
                : signedAngle);
        }
    }

    private void SetCurrentAnimation(string newAnimation) {
        string oldAnimation = currentAnimation;
        currentAnimation = newAnimation;
        if (oldAnimation != currentAnimation) {
            currentFrame = 0;
            newFrameStartTime = Time.time + secondsPerFrame;
        }
    }

    private void Update() {
        var categoryAndLabel = TryGetCurrentCategoryAndLabel();
        if (categoryAndLabel == null)
            return;

        string category = categoryAndLabel.Value.Item1;
        string label = categoryAndLabel.Value.Item2;
        
        if (Time.time >= newFrameStartTime) {
            currentFrame++;
            if (currentFrame >= categoriesAndLabels[category].Length)
                currentFrame = 0;
            newFrameStartTime += secondsPerFrame;

            // Debug.Log($"Next frame {category} {label}");

            spriteResolver.SetCategoryAndLabel(category,
                label);
            spriteResolver.ResolveSpriteToSpriteRenderer();
        }
    }
}
