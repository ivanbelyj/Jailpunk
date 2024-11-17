using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private readonly SpriteLibraryDecorator spriteLibraryDecorator;

    // Static to log every not existing category only once
    // regardless of the count of element renderers
    private static readonly HashSet<string> loggedWarningCategories = new();

    private readonly SortingOrderHelper sortingOrderHelper;
    private bool isActive = true;

    public string Name => appearanceElementSchema.name;

    public GameObject RendererGameObject => spriteRenderer.gameObject;

    public AppearanceElementRenderer(
        AppearanceSchema appearanceSchema,
        AppearanceElementSchema appearanceElementSchema,
        SpriteRenderer spriteRenderer,
        SpriteMask spriteMask,
        SpriteLibraryDecorator spriteLibraryDecorator)
    {
        this.appearanceSchema = appearanceSchema;
        this.appearanceElementSchema = appearanceElementSchema;
        this.spriteRenderer = spriteRenderer;
        this.spriteMask = spriteMask;
        this.spriteLibraryDecorator = spriteLibraryDecorator;

        sortingOrderHelper = new(appearanceElementSchema);
    }

    public void Apply(AppearanceElementData data) {
        spriteRenderer.color = data.color;
        SetActive(data.isActive);
    }
    
    public void SetActive(bool isActive) {
        this.isActive = isActive;

        SetActiveCore(isActive);
    }

    private void SetActiveCore(bool isActive) {
        spriteRenderer.enabled
            = spriteMask.enabled 
            = this.isActive && isActive;
    }

    public void Render(AppearanceRenderData appearanceRenderData) {
        if (!isActive) {
            return;
        }
        
        var categoryAndLabel = TryGetCurrentCategoryAndLabel(appearanceRenderData);

        SetActiveCore(categoryAndLabel != null);
        
        if (categoryAndLabel == null) {
            return;
        }

        string category = categoryAndLabel.Value.category;
        string label = categoryAndLabel.Value.label;

        RenderCore(appearanceRenderData, category, label);
    }

    private void RenderCore(
        AppearanceRenderData appearanceRenderData,
        string category,
        string label) {
        spriteRenderer.sprite
            = spriteMask.sprite
            = spriteLibraryDecorator.GetSprite(category, label);
        
        Flip(appearanceRenderData.FlipX);

        spriteRenderer.transform.localPosition
            = spriteMask.transform.localPosition
            = GetLocalPosition(appearanceRenderData);

        SetSortingOrder(appearanceRenderData.Angle);
    }

    private void SetSortingOrder(int angle) {
        spriteRenderer.sortingOrder = sortingOrderHelper.GetSortingOrder(angle);
    }

    private void Flip(bool flipX) {
        spriteRenderer.flipX = flipX;
        spriteMask.transform.localEulerAngles = new (
            spriteMask.transform.localRotation.x,
            spriteMask.transform.localRotation.y - (flipX ? 180 : 0),
            spriteMask.transform.localRotation.z);
    }

    private Vector3 GetLocalPosition(AppearanceRenderData appearanceRenderData) {

        float onePixel = 1f / spriteRenderer.sprite.pixelsPerUnit;
        Vector2Int offset = GetOffset(appearanceRenderData);
        return new Vector3(
            offset.x * onePixel,
            offset.y * onePixel,
            spriteRenderer.gameObject.transform.localPosition.z);
    }

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

    private bool HandleStopFrame(
        ref AppearanceRenderData appearanceRenderData) {
        if (appearanceElementSchema.useStopFrames
            && appearanceRenderData.State == StateIdle
            && appearanceRenderData.LastWalkFrame != null) { 
            appearanceRenderData.State = StateWalk;
            appearanceRenderData.Frame = GetClosestStopFrame(
                appearanceRenderData.LastWalkFrame.Value,
                GetFramesCount(appearanceRenderData.State));
            return true;
        }
        return false;
    }

    private (string category, string label)? TryGetCurrentCategoryAndLabel(
        AppearanceRenderData appearanceRenderData) {
        bool usingStopFrame = HandleStopFrame(ref appearanceRenderData);

        var frame = appearanceRenderData.Frame;
        var state = appearanceRenderData.State;
        var angle = appearanceRenderData.Angle;

        if (frame < 0)
            return null;

        string categoryName = SpriteLibraryUtils.GetCategoryName(
            appearanceElementSchema.name,
            state: appearanceElementSchema.ignoreState ? null : state,
            angle: appearanceElementSchema.ignoreAngle ? null : angle
        );

        string category = spriteLibraryDecorator.GetCategoryOrNull(
            categoryName);

        if (category == null) {
            if (usingStopFrame) {
                if (!loggedWarningCategories.Contains(categoryName)) {
                    Debug.LogWarning($"Cannot render appearance element: "
                        + $"category {categoryName} doesn't exist");
                    loggedWarningCategories.Add(categoryName);
                }
            }

            return null;
        }
        
        string[] labels = spriteLibraryDecorator.GetLabels(category);

        if (labels.Length == 0) {
            throw new InvalidOperationException(
                "Category " + category + " doesn't contain any labels");
        }

        return (category, GetLabel(frame, labels));
    }

    private string GetLabel(int frame, string[] labels) {
        return labels[frame % labels.Length];
    }

    private int GetFramesCount(string state)
        => appearanceSchema.animationSchema.GetStateSchema(state).framesCount;

    private Vector2Int GetOffset(AppearanceRenderData appearanceRenderData) {
        if (appearanceElementSchema.useOrigin) {
            return GetOriginOffset(appearanceRenderData);
        }

        return Vector2Int.zero;
    }

    private Vector2Int GetOriginOffset(AppearanceRenderData appearanceRenderData) {
        var origin = appearanceSchema.animationSchema
            .GetOriginByName(appearanceElementSchema.originName);
            
        var offset = GetOriginItemOffset(origin, appearanceRenderData);
        
        if (appearanceRenderData.FlipX) {
            offset.x *= -1;
        }
        return offset;
    }

    private Vector2Int GetOriginItemOffset(
        AnimatedOrigin origin,
        AppearanceRenderData appearanceRenderData) {
        var originItem = origin
            .GetItem(appearanceRenderData.State, appearanceRenderData.Angle);
        
        // TODO: Handle or fix that LastWalkFrame was not set initially
        if (originItem != null
            && originItem.state == StateIdle
            && appearanceRenderData.LastWalkFrame == null) {
            // Debug.LogWarning(
            //     $"{nameof(appearanceRenderData.LastWalkFrame)} was not set, " +
            //     $"but it's required for idle animation");
            return Vector2Int.zero;
        }
        return originItem
            ?.GetOffset(
                originItem.state == StateIdle
                ? appearanceRenderData.LastWalkFrame.Value
                : appearanceRenderData.Frame)
            ?? Vector2Int.zero;
    }
}
