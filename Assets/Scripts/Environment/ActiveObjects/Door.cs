using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum DoorType {
    Single,
    Double
}

public class Door : InteractableObject, IRenewable
{
    [SerializeField]
    private DoorType doorType;
    
    [SerializeField]
    private CustomizableAppearance customizableAppearance;

    [SerializeField]
    private AppearanceAnimator appearanceAnimator;

    [SerializeField]
    [Tooltip("Collider serving as an obstacle")]
    private BoxCollider2D obstacleBoxCollider;

    [SerializeField]
    [Tooltip("Collider alowing to click on the door")]
    private BoxCollider2D selectionBoxCollider;

    private GridTransform gridTransform;

    private bool isOpened;

    public bool IsInRenewedState => isOpened;

    protected override void Awake()
    {
        base.Awake();
        gridTransform = GetComponent<GridTransform>();

        customizableAppearance.AppearanceElementsReady += OnAppearanceElementsSet;
    }

    protected override void Start()
    {
        base.Start();
        SetCurrentAnimationState("default");
        AdjustColliders();
        gridTransform.FirstOrientationSet += (sender, orientation) => {
            // Default animation is commonly single-frame, so it renders just one time.
            // We should provide correct angle
            // SetCurrentAnimationState("default");    
            // AdjustCollider();
        };
    }

    private void OnAppearanceElementsSet(CustomizableAppearance appearance) {
        FlashEffectComponents = GetFlashEffectComponents(appearance);
    }

    // Todo: move out from the Door component to something more universal ?
    private List<FlashEffect> GetFlashEffectComponents(CustomizableAppearance appearance)
    {
        return appearance
            .GetElements()
            .Select(x => x.GetComponent<FlashEffect>())
            .Where(x => x != null)
            .ToList();
    }

    public void Open() {
        if (isOpened)
            return;

        SetCurrentAnimationState("opening");

        obstacleBoxCollider.isTrigger = true;

        State = ActivationState.ReadyToActivate;  // Door can be closed
        isOpened = true;
    }

    public void Close() {
        if (!isOpened)
            return;

        SetCurrentAnimationState("closing");

        obstacleBoxCollider.isTrigger = false;
        
        State = ActivationState.ReadyToActivate;
        isOpened = false;
    }


    public void Renew()
    {
        Close();
    }

    protected override void Activate()
    {
        if (isOpened)
            Close();
        else
            Open();
    }

    private void AdjustColliders() {
        Vector2 obstacleColliderSize;
        Vector2 obstacleColliderOffset;
        Vector2 selectionColliderSize;
        Vector2 selectionColliderOffset;
        switch (doorType) {
            case DoorType.Single:
                obstacleColliderSize = new Vector2(1, 1);
                obstacleColliderOffset = new Vector2(0, 0.5f);

                selectionColliderSize = new Vector2(1, 2);
                selectionColliderOffset = new Vector2(0, 1);
                break;
            case DoorType.Double:
                var isDoorVertical = GridDirectionUtils.IsVertical(gridTransform.Orientation);
                obstacleColliderSize = new Vector2(isDoorVertical ? 2 : 1, isDoorVertical ? 1 : 2);
                obstacleColliderOffset = new Vector2(0, isDoorVertical ? 0.5f : 1);

                selectionColliderSize = new Vector2(isDoorVertical ? 2 : 1, 2);
                selectionColliderOffset = new Vector2(0, 1);
                break;
            default:
                throw new NotSupportedException(
                    $"{nameof(DoorType)} value is not supported");
        }

        obstacleBoxCollider.size = obstacleColliderSize;
        obstacleBoxCollider.offset = obstacleColliderOffset;

        selectionBoxCollider.size = selectionColliderSize;
        selectionBoxCollider.offset = selectionColliderOffset;
    }

    private void SetCurrentAnimationState(string animationState) {
        appearanceAnimator.SetCurrentAnimation(new() {
            State = animationState,
            Angle = GridDirectionUtils.GridDirectionToAnimationAngle(gridTransform.Orientation),
        });
    }
}
