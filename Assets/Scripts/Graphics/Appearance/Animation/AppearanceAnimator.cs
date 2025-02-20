using UnityEngine;

using static AnimationConstants;

[RequireComponent(typeof(IAppearance))]
public class AppearanceAnimator : MonoBehaviour
{
    protected IAppearance appearance;

    protected FrameUpdater frameUpdater;

    private bool isCurrentAnimationSet = false;
    private AppearanceAnimationData appearanceAnimationDataToSet;
    
    protected virtual void Awake() {
        appearance = GetComponent<IAppearance>();
        frameUpdater = new FrameUpdater(appearance);
    }

    protected virtual void Update() {
        if (!isCurrentAnimationSet) {
            frameUpdater.SetCurrentAnimation(appearanceAnimationDataToSet);
            isCurrentAnimationSet = true;
        }
        frameUpdater.Update();
    }

    public void SetCurrentAnimation(AppearanceAnimationData appearanceAnimationData) {
        isCurrentAnimationSet = false;
        this.appearanceAnimationDataToSet = appearanceAnimationData;
    }
}
