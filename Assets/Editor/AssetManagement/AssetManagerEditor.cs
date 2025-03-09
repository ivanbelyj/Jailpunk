// using UnityEditor;
// using UnityEngine;

// #if UNITY_EDITOR
// [CustomEditor(typeof(AssetManager))]
// public class AssetManagerEditor : Editor
// {
//     private SerializedProperty assetBundleNamesProperty;
//     private AssetManager targetManager;

//     private void OnEnable()
//     {
//         targetManager = (AssetManager)target;
//         assetBundleNamesProperty = serializedObject.FindProperty("assetBundleNames");
//     }

//     public override void OnInspectorGUI()
//     {
//         serializedObject.Update();

//         EditorGUILayout.PropertyField(assetBundleNamesProperty);

//         if (GUILayout.Button("Update AssetBundles"))
//         {
//             UpdateAssetBundleNames();
//         }

//         serializedObject.ApplyModifiedProperties();
//     }

//     private void UpdateAssetBundleNames()
//     {
//         string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
//         targetManager.assetBundleNames.Clear();
//         targetManager.assetBundleNames.AddRange(assetBundleNames);
//         EditorUtility.SetDirty(targetManager);
//         AssetDatabase.SaveAssets();
//     }
// }
// #endif
