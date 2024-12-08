using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeBuilding : GenerationStage
{
    [SerializeField]
    private GameObject gridPrefab;

    [SerializeField]
    private GameObject floorPrefab;

    [SerializeField]
    private GameObject wallsPrefab;

    [SerializeField]
    private TileProvider tileProvider;

    public override GenerationContext ProcessMaze(GenerationContext context)
    {
        if (!ValidatePrefabs())
        {
            Debug.LogError("Prefabs are not assigned properly.");
            return context;
        }

        MazeScheme mazeScheme = context.MazeData.Scheme;

        GameObject gridObject = CreateGrid(context.RootGameObjectName);
        Tilemap floorTilemap = CreateTilemap(gridObject, floorPrefab, "Floor");
        Tilemap wallTilemap = CreateTilemap(gridObject, wallsPrefab, "Walls");

        PopulateTilemaps(mazeScheme, floorTilemap, wallTilemap);

        return context;
    }

    private bool ValidatePrefabs()
    {
        return gridPrefab != null && floorPrefab != null && wallsPrefab != null;
    }

    private GameObject CreateGrid(string gameObjectName)
    {
        GameObject gridObject = Instantiate(gridPrefab, Vector3.zero, Quaternion.identity);
        gridObject.name = gameObjectName;
        return gridObject;
    }

    private Tilemap CreateTilemap(GameObject parent, GameObject prefab, string name)
    {
        GameObject tilemapObject = Instantiate(prefab, parent.transform);
        tilemapObject.name = name;
        return tilemapObject.GetComponent<Tilemap>();
    }

    private void PopulateTilemaps(MazeScheme mazeScheme, Tilemap floorTilemap, Tilemap wallTilemap)
    {
        for (int y = 0; y < mazeScheme.MapSize.y; y++)
        {
            for (int x = 0; x < mazeScheme.MapSize.x; x++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                PlaceTile(
                    mazeScheme.GetTileByPos(x, y),
                    position,
                    floorTilemap,
                    wallTilemap);
            }
        }

        floorTilemap.RefreshAllTiles();
        wallTilemap.RefreshAllTiles();
    }

    private void PlaceTile(SchemeTile schemeTile, Vector3Int position, Tilemap floorTilemap, Tilemap wallTilemap)
    {
        // var tile = GetTile(schemeTile.TileId);
        switch (schemeTile.TileType)
        {
            case TileType.Floor:
                // TODO: temporary hardcoded getting tile
                floorTilemap.SetTile(position, GetTile(1)); 
                break;
            case TileType.Wall:
            case TileType.LoadBearingWall:
                wallTilemap.SetTile(position, GetTile(2));
                break;
        }
    }

    private Tile GetTile(uint tileId) {
        return tileProvider.GetTile(tileId);
    }
}
