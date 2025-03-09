using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ComplexBuilding : GenerationStage
{
    [SerializeField]
    private GameObject gridPrefab;

    [SerializeField]
    private GameObject floorTilemapPrefab;

    [SerializeField]
    private GameObject obstacleTilemapPrefab;

    [SerializeField]
    private GameObject visionObstacleTilemapPrefab;

    [SerializeField]
    private TileProvider tileProvider;

    #region Set on run stage
    private AssetManager assetManager;
    
    private GameObject gridObject;
    private Tilemap floorTilemap;
    private Tilemap obstacleTilemap;
    private Tilemap visionObstacleTilemap;
    #endregion

    public override void RunStage()
    {
        if (!ValidatePrefabs())
        {
            Debug.LogError(
                $"Prefabs are not assigned properly for {nameof(ComplexBuilding)} " +
                "generation stage");
            return;
        }

        Initialize();

        PopulateTilemaps(context.ComplexData.Scheme);
    }

    private void Initialize() {
        assetManager = FindAnyObjectByType<AssetManager>();

        gridObject = CreateGrid(context.RootGameObjectName);
        floorTilemap = CreateTilemap(gridObject, floorTilemapPrefab, "Floor");
        obstacleTilemap = CreateTilemap(gridObject, obstacleTilemapPrefab, "Obstacles");
        visionObstacleTilemap = CreateTilemap(gridObject, visionObstacleTilemapPrefab, "VisionObstacles");

        GenerationData.InstantiatedComplexData = new() {
            GridGameObject = gridObject,
            ObstacleTilemap = obstacleTilemap,
            VisionObstacleTilemap = visionObstacleTilemap
        };
    }

    private bool ValidatePrefabs()
    {
        return gridPrefab != null
            && floorTilemapPrefab != null
            && obstacleTilemapPrefab != null
            && visionObstacleTilemapPrefab != null;
    }

    private GameObject CreateGrid(string gameObjectName)
    {
        GameObject gridObject = Instantiate(gridPrefab, Vector3.zero, Quaternion.identity);
        gridObject.name = gameObjectName;
        gridObject.transform.localPosition = context.Settings.complexPosition;
        return gridObject;
    }

    private Tilemap CreateTilemap(GameObject parent, GameObject prefab, string name)
    {
        GameObject tilemapObject = Instantiate(prefab, parent.transform);
        tilemapObject.name = name;
        return tilemapObject.GetComponent<Tilemap>();
    }

    private void PopulateTilemaps(ComplexScheme mazeScheme)
    {
        for (int y = 0; y < mazeScheme.MapSize.y; y++)
        {
            for (int x = 0; x < mazeScheme.MapSize.x; x++)
            {
                Vector2Int position = new Vector2Int(x, y);
                PlacePosition(mazeScheme.GetTileByPos(x, y), position);
            }
        }

        ResetTilemaps();        
    }

    private void ResetTilemaps() {
        var tilemaps = new List<Tilemap>() {
            floorTilemap, obstacleTilemap, visionObstacleTilemap
        };
        foreach (var tilemap in tilemaps) {
            tilemap.RefreshAllTiles();
            tilemap.CompressBounds();
        }
    }

    private void PlacePosition(
        SchemePosition schemePosition,
        Vector2Int position)
    {
        // TODO: uncomment later
        // for (int i = 0; i < schemePosition.Layers.Count; i++) {
        //     var layer = schemePosition.Layers[i];
            // TODO: temporary hardcoded
            var layer = new SchemeTile() {
                MapObjectName = schemePosition.Type switch {
                    SchemePositionType.Floor => "floor",
                    SchemePositionType.Wall or SchemePositionType.LoadBearingWall => "wall",
                    _ => null
                }
            };

            var mapObjectSchema = GetMapObjectSchema(layer.MapObjectName);

            var tile = GetTile(layer);
            var tilemap = SelectTilemap(mapObjectSchema);

            var layeredPosition = new Vector3Int(position.x, position.y, 0);
            tilemap.SetTile(layeredPosition, tile);
        // }
    }

    private Tilemap SelectTilemap(MapObjectSchema mapObjectSchema)
    {
        return mapObjectSchema.obstacleType switch {
            MapObjectObstacleType.Passable => floorTilemap,
            MapObjectObstacleType.Obstacle => obstacleTilemap,
            MapObjectObstacleType.VisionObstacle => visionObstacleTilemap,
            _ => throw new ArgumentException()
        };
    }

    private MapObjectSchema GetMapObjectSchema(string mapObjectName)
    {
        return assetManager.MapObjectSchemas.GetAssetById(mapObjectName);
    }

    private Tile GetTile(SchemeTile schemeTile) {
        return tileProvider.GetTile(schemeTile);
    }
}
