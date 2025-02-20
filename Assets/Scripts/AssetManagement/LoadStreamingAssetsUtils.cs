using UnityEngine;

public static class LoadStreamingAssetsUtils
{
    public static T[] LoadStreamingAssets<T>(string bundleName)
        where T : Object
    {
        AssetBundle localAssetBundle = AssetBundle.LoadFromFile(
            System.IO.Path.Combine(Application.streamingAssetsPath, bundleName)
        );

        if (localAssetBundle == null) {
            Debug.LogError($"Failed to load AssetBundle (name: {bundleName})");
            return System.Array.Empty<T>();
        }
        
        T[] assets = localAssetBundle.LoadAllAssets<T>();

        localAssetBundle.Unload(false);

        return assets;
    }
}
