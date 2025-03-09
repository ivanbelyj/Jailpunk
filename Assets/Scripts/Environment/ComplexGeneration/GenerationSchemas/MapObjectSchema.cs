using UnityEngine;

public enum MapObjectInstantiationType
{
    [Tooltip(
        "Will be placed as tile on the map. Used for not interactable object " +
        "and more efficient")]
    Tile,

    [Tooltip("Will be instantiated as an individual GameObject")]
    Prefab
}

public enum MapObjectObstacleType
{
    [Tooltip("An object that can be passed without obstacles")]
    Passable,
    [Tooltip("A collidable object that does not obstruct visibility")]
    Obstacle,
    [Tooltip("A collidable object through which there is no visibility")]
    VisionObstacle
}

public enum MapObjectAppearanceResolvingType
{
    [Tooltip("Use random sprite defined in appearance (new random index for every tile)")]
    RandomIndex,
    [Tooltip(
        "Use random sprite defined in appearance, a single for all the objects " +
        "(single random sprite for all tiles)")]
    RandomIndexForAll,
    [Tooltip("Use defined index and ignore other appearance sprite data")]
    ByIndex,
    [Tooltip("Use full appearance sprite data")]
    ByFullAppearanceSpriteData
}

[CreateAssetMenu(
    fileName = "New Map Object Schema",
    menuName = "Complex Generation/Map Object Schema",
    order = 52)]
public class MapObjectSchema : ScriptableObject
{
    public MapObjectInstantiationType instantiationType;

    [Header("Tile-instantiated object")]
    public MapObjectObstacleType obstacleType = MapObjectObstacleType.Obstacle;

    public MapObjectAppearanceResolvingType mapObjectAppearanceResolvingType = MapObjectAppearanceResolvingType.RandomIndex;
    public AppearanceSpriteData appearanceSpriteData = new() {
        Name = "new-map-object-appearance-name"
    };

    [Tooltip("Map objects with higher order are rendered above others")]
    public int sortingOrder = 0;
    
    public Color color = Color.white;
}
