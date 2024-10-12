using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A component that allows to control the maze
/// </summary>
public class Maze : MonoBehaviour
{
    [SerializeField]
    private RenewalFeature renewalFeature;
    public RenewalFeature RenewalFeature { get => renewalFeature; }

    private IEnumerable<IActivatable> activatables;
    /// <summary>
    /// All contolled objects that can be activated in the maze
    /// (not necessarily every activatable object)
    /// </summary>
    public IEnumerable<IActivatable> Activatables => activatables;

    [SerializeField]
    private bool findActivatablesOnScene;

    private void Awake() {
        InitializeMazeFeatures();
        if (findActivatablesOnScene) {
            activatables = FindImplementations<IActivatable>();
        }
    }

    private void InitializeMazeFeatures() {
        var mazeFeatures = new List<MazeFeature>();
        mazeFeatures.Add(renewalFeature);

        foreach (var feature in mazeFeatures) {
            feature.Initialize(this);
        }
    }

    public static IEnumerable<T> FindImplementations<T>(
        bool includeInactive = false) =>
        SceneManager
        .GetActiveScene()
        .GetRootGameObjects()
        .SelectMany(go => go.GetComponentsInChildren<T>(includeInactive));
    
}
