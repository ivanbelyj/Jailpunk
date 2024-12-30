using UnityEngine;

public enum MapObjectInstantiationType {
    [Tooltip(
        "Will be placed as tile on the map. Used for not interactable object " +
        "and more efficient")]
    Tile,

    [Tooltip("Will be instantiated as an individual GameObject")]
    Prefab
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
}
