using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseAssetManager<T> : MonoBehaviour
    where T : Object
{
    [SerializeField]
    protected string bundleName = "assetbundlename";
    protected Dictionary<string, T> assetsById;

    private void Awake() {
        var assets = LoadStreamingAssetsUtils.LoadStreamingAssets<T>(bundleName);
        assetsById = assets.ToDictionary(GetAssetId, x => x);
    }

    public T GetAssetById(string id) => assetsById[id];

    abstract protected string GetAssetId(T asset);
}
