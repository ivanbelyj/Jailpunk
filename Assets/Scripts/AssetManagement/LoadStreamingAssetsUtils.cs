using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class LoadStreamingAssetsUtils
{
    public static UnityEngine.Object[] LoadStreamingAssets(string bundleName)
    {
        AssetBundle localAssetBundle = AssetBundle.LoadFromFile(
            Path.Combine(Application.streamingAssetsPath, bundleName)
        );

        if (localAssetBundle == null) {
            Debug.LogError($"Failed to load AssetBundle (name: {bundleName})");
            return Array.Empty<UnityEngine.Object>();
        }
        
        UnityEngine.Object[] assets = localAssetBundle.LoadAllAssets();
            // localAssetBundle
            // .GetAllAssetNames()
            // .Select(x => localAssetBundle.LoadAssetWithSubAssets(x).First())
            // .ToArray();

        localAssetBundle.Unload(false);

        return assets;
    }

    // /// <summary>
    // /// Names of bundles existing in StreamingAssets folder
    // /// </summary>
    // public static IEnumerable<string> GetAllAssetBundleNames()
    // {
    //     try
    //     {
    //         var allFiles = Directory.GetFiles(Application.streamingAssetsPath);
    //         return allFiles
    //             .Where(path => !string.IsNullOrEmpty(Path.GetFileName(path)))
    //             .Select(Path.GetFileNameWithoutExtension)
    //             .ToList();
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogError($"Error getting asset bundle files: {e.Message}");
    //         return Enumerable.Empty<string>();
    //     }
    // }
}
