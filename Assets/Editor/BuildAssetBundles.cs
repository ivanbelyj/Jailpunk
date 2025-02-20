using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildAssetBundles : MonoBehaviour
{
    [MenuItem("Assets/Build AssetBundles")]
	private static void BuildAllAssetBundles()
	{
        string assetBundleDirectory = "Assets/StreamingAssets";
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        var manifest = BuildPipeline.BuildAssetBundles(
            assetBundleDirectory,
            BuildAssetBundleOptions.None,
            EditorUserBuildSettings.activeBuildTarget);

        Debug.Log($"Streaming assets were built. {string.Join(", ", manifest.GetAllAssetBundles())}");
	}
}
