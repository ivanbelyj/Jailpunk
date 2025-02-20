using System;
using UnityEngine;

using static AnimationConstants;

public class AppearanceElementRenderer
{
    private readonly AppearanceSchema appearanceSchema;
    private readonly AppearanceElementSchema appearanceElementSchema;
    private readonly SpriteRenderer spriteRenderer;

    /// <summary>
    /// Sprite mask is used to hide darkness behind the character in some cases
    /// </summary>
    private readonly SpriteMask spriteMask;
    private readonly AppearanceSpriteResolver appearanceSpriteResolver;
    private readonly IAppearanceElementNameResolver appearanceElementNameResolver;
    private readonly AppearanceSortingOrderHelper sortingOrderHelper;
    private readonly AppearanceElementOffsetHelper offsetHelper;
    private bool isActive = true;

    public string Name => appearanceElementSchema.name;

    public GameObject RendererGameObject => spriteRenderer.gameObject;

    public AppearanceElementRenderer(
        AppearanceSchema appearanceSchema,
        AppearanceElementSchema appearanceElementSchema,
        SpriteRenderer spriteRenderer,
        SpriteMask spriteMask,
        AppearanceSpriteResolver appearanceSpriteResolver,
        IAppearanceElementNameResolver appearanceElementNameResolver)
    {
        this.appearanceSchema = appearanceSchema;
        this.appearanceElementSchema = appearanceElementSchema;
        this.spriteRenderer = spriteRenderer;
        this.spriteMask = spriteMask;
        this.appearanceSpriteResolver = appearanceSpriteResolver;
        this.appearanceElementNameResolver = appearanceElementNameResolver;
        sortingOrderHelper = new(appearanceElementSchema);
        offsetHelper = new(appearanceSchema, appearanceElementSchema);
    }

    public void Apply(AppearanceElementConfig data) {
        spriteRenderer.color = data.color;
        SetActive(data.isActive);
    }
    
    public void SetActive(bool isActive) {
        this.isActive = isActive;

        ApplyIsActive(isActive);
    }

    private void ApplyIsActive(bool isActive) {
        spriteRenderer.enabled
            = spriteMask.enabled 
            = this.isActive && isActive;
    }

    public void Render(AppearanceAnimationData animationData) {
        if (!isActive) {
            return;
        }
        
        var elementData = GetElementRenderData(animationData);
        var sprite = appearanceSpriteResolver.Resolve(elementData.SpriteData);

        ApplyIsActive(sprite != null);
        
        if (sprite == null) {
            return;
        }

        RenderCore(elementData, sprite, animationData);
    }

    private void RenderCore(
        AppearanceElementRenderData data,
        Sprite sprite,
        AppearanceAnimationData animationData)
    {
        spriteRenderer.sprite = spriteMask.sprite = sprite;
        
        Flip(data.FlipX);

        spriteRenderer.transform.localPosition
            = spriteMask.transform.localPosition
            = GetLocalPosition(data, animationData);

        SetSortingOrderByAngle(animationData.Angle);
    }

    private void SetSortingOrderByAngle(int angle) {
        spriteRenderer.sortingOrder = sortingOrderHelper.GetSortingOrder(angle);
    }

    private void Flip(bool flipX) {
        spriteRenderer.flipX = flipX;
        spriteMask.transform.localEulerAngles = new (
            spriteMask.transform.localRotation.x,
            spriteMask.transform.localRotation.y - (flipX ? 180 : 0),
            spriteMask.transform.localRotation.z);
    }

    private Vector3 GetLocalPosition(
        AppearanceElementRenderData data,
        AppearanceAnimationData animationData) {

        float onePixel = 1f / spriteRenderer.sprite.pixelsPerUnit;
        Vector2Int offset = offsetHelper.GetOffset(data, animationData);
        return new Vector3(
            offset.x * onePixel,
            offset.y * onePixel,
            spriteRenderer.gameObject.transform.localPosition.z);
    }

    private AppearanceElementRenderData GetElementRenderData(
        AppearanceAnimationData animationData)
    {
        var spriteData = new AppearanceSpriteData() {
            Name = appearanceElementNameResolver.GetAppearanceSpriteName(
                appearanceElementSchema.name),
            State = appearanceElementSchema.ignoreState ? null : animationData.State,
            Angle = appearanceElementSchema.ignoreAngle ? null : animationData.Angle,
            Index = animationData.Frame
        };

        ApplyStopFrame(ref spriteData, animationData);

        var elementData = new AppearanceElementRenderData() {
            SpriteData = spriteData,
            FlipX = animationData.Flip
        };
        return elementData;
    }

    private void ApplyStopFrame(
        ref AppearanceSpriteData spriteData,
        AppearanceAnimationData animationData)
    {
        if (appearanceElementSchema.useStopFrames
            && animationData.State == StateIdle
            && animationData.LastWalkFrame != null)
        {
            spriteData.State = StateWalk;
            spriteData.Index = GetClosestStopFrame(
                animationData.LastWalkFrame.Value,
                GetFramesCount(animationData.State));
        }
    }

    private int GetFramesCount(string state)
        => appearanceSchema.animationSchema.GetStateSchema(state).framesCount;

    private int[] GetStopFrames()
        => appearanceSchema
            .GetStopFramesSetByName(appearanceElementSchema.stopFramesSet)
            .stopFrames;

    private int GetClosestStopFrame(int lastWalkFrame, int framesCount) {
        var stopFrames = GetStopFrames();
        int minDistance = int.MaxValue;
        int minStopFrame = stopFrames.Length > 1 ? stopFrames[0] : lastWalkFrame;
        foreach (int x in stopFrames) {
            int distance = DistanceBetweenFrames(
                x,
                lastWalkFrame,
                framesCount);
            if (distance < minDistance) {
                minDistance = distance;
                minStopFrame = x;
            }
        }
        return minStopFrame;
    }

    private static int DistanceBetweenFrames(int index1, int index2, int length) {
        int max = Math.Max(index1, index2);
        int min = Math.Min(index1, index2);
        return Math.Min(max - min, length - max + min);
    }
}
