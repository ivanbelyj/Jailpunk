using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TigerForge;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetDictionary<T> where T : UnityEngine.Object
{
    protected Dictionary<string, AsyncOperationHandle<T>> assetHandlesById;
    protected Dictionary<string, T> loadedAssets;

    public AssetDictionary()
    {
        assetHandlesById = new Dictionary<string, AsyncOperationHandle<T>>();
        loadedAssets = new Dictionary<string, T>();
    }

    public async Task Initialize(IEnumerable<string> assetAddresses)
    {
        foreach (var address in assetAddresses)
        {
            var handle = Addressables.LoadAssetAsync<T>(address);
            assetHandlesById[address] = handle;
        }

        await Task.WhenAll(assetHandlesById.Values.Select(x => x.Task));

        foreach (var (key, value) in assetHandlesById)
        {
            if (value.Status == AsyncOperationStatus.Succeeded)
            {
                loadedAssets[key] = value.Result;
            }
        }
    }

    public T GetAssetById(string id)
    {
        if (loadedAssets.TryGetValue(id, out T asset))
        {
            return asset;
        }
        throw new KeyNotFoundException($"Asset with id {id} not found");
    }

    public int Count => loadedAssets.Count;

    public void Cleanup()
    {
        foreach (var handle in assetHandlesById.Values)
        {
            Addressables.Release(handle);
        }
        assetHandlesById.Clear();
        loadedAssets.Clear();
    }
}

public class AssetManager : MonoBehaviour
{
    public AssetDictionary<ComplexGenerationSchema> ComplexGenerationSchemas { get; private set; }
    public AssetDictionary<SectorGenerationSchema> SectorGenerationSchemas { get; private set; }
    public AssetDictionary<SectorZoneGenerationSchema> SectorZoneGenerationSchemas { get; private set; }
    public AssetDictionary<MapObjectSchema> MapObjectSchemas { get; private set; }

    private async void Start()
    {
        await Initialize();
    }

    private async Task Initialize()
    {
        var complexTask = InitializeAssetDictionary<ComplexGenerationSchema>("complex-generation-schema");
        var sectorTask = InitializeAssetDictionary<SectorGenerationSchema>("sector-generation-schema");
        var sectorZoneTask = InitializeAssetDictionary<SectorZoneGenerationSchema>("sector-zone-generation-schema");
        var mapObjectTask = InitializeAssetDictionary<MapObjectSchema>("map-object-schema");

        await Task.WhenAll(complexTask, sectorTask, sectorZoneTask, mapObjectTask);

        ComplexGenerationSchemas = await complexTask;
        SectorGenerationSchemas = await sectorTask;
        SectorZoneGenerationSchemas = await sectorZoneTask;
        MapObjectSchemas = await mapObjectTask;

        EventManager.EmitEvent(EventConstants.ComplexGenerationAssetsLoaded);
    }

    private async Task<AssetDictionary<T>> InitializeAssetDictionary<T>(string label) where T : UnityEngine.Object
    {
        var assetDictionary = new AssetDictionary<T>();
        await assetDictionary.Initialize(await GetAllAddressablesByLabelAsync<T>(label));
        LogInfo(typeof(T).Name, assetDictionary.Count);
        return assetDictionary;
    }

    private void LogInfo(string name, int count)
    {
        Debug.Log($"Loaded {count} assets of type: {name}");
    }

    private async Task<List<string>> GetAllAddressablesByLabelAsync<T>(string label) where T : UnityEngine.Object
    {
        var locations = await Addressables.LoadResourceLocationsAsync(label).Task;
        return locations.Select(location => location.PrimaryKey).ToList();
    }

    private void OnDestroy()
    {
        ComplexGenerationSchemas?.Cleanup();
        SectorGenerationSchemas?.Cleanup();
        SectorZoneGenerationSchemas?.Cleanup();
        MapObjectSchemas?.Cleanup();
    }
}
