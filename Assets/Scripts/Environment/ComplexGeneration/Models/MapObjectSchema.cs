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

[CreateAssetMenu(
    fileName = "New Map Object",
    menuName = "Procedural Maze/Map Object",
    order = 52)]
public class MapObjectSchema : ScriptableObject
{
    public string mapObjectName = "new-map-object-name";
    public MapObjectInstantiationType instantiationType;

    [Header("Appearance")]
    public string appearanceName = "new-map-object-name";

    public MapObjectObstacleType obstacleType = MapObjectObstacleType.Obstacle;
}
