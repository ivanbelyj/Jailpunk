using UnityEngine;

using static AnimationConstants;

public class AppearanceElementOffsetHelper
{
    private readonly AppearanceSchema appearanceSchema;
    private readonly AppearanceElementSchema appearanceElementSchema;

    public AppearanceElementOffsetHelper(
        AppearanceSchema appearanceSchema,
        AppearanceElementSchema appearanceElementSchema)
    {
        this.appearanceSchema = appearanceSchema;
        this.appearanceElementSchema = appearanceElementSchema;
    }

    public Vector2Int GetOffset(
        AppearanceElementRenderData data,
        AppearanceAnimationData animationData) {
        if (appearanceElementSchema.useOrigin) {
            return GetOriginOffset(data, animationData);
        }

        return Vector2Int.zero;
    }

    private Vector2Int GetOriginOffset(
        AppearanceElementRenderData data,
        AppearanceAnimationData animationData) {
        var origin = appearanceSchema.animationSchema
            .GetOriginByName(appearanceElementSchema.originName);
            
        var offset = GetOriginItemOffset(origin, data, animationData);
        
        if (data.FlipX) {
            offset.x *= -1;
        }
        return offset;
    }

    private Vector2Int GetOriginItemOffset(
        AnimatedOrigin origin,
        AppearanceElementRenderData data,
        AppearanceAnimationData animationData)
    {
        var originItem = origin.GetItem(
            animationData.State,
            animationData.Angle);
        
        // TODO: Handle or fix that LastWalkFrame was not set initially
        if (originItem != null
            && originItem.state == StateIdle
            && animationData.LastWalkFrame == null) {
            // Debug.LogWarning(
            //     $"{nameof(appearanceRenderData.LastWalkFrame)} was not set, " +
            //     $"but it's required for idle animation");
            return Vector2Int.zero;
        }

        var result = originItem
            ?.GetOffset(
                originItem.state == StateIdle
                ? animationData.LastWalkFrame.Value
                : data.SpriteData.Frame.Value)
            ?? Vector2Int.zero;
            
        return result;
    }
}
